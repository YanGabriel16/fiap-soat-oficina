using Oficina.Domain.Common;
using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public sealed class OrcamentoService(
    IOrcamentoRepository repository,
    IOrdemServicoRepository ordemRepository,
    IClienteRepository clienteRepository,
    IItemCatalogoRepository itemRepository,
    Oficina.Infra.Data.Context.OficinaDbContext db,
    INotificationQueue notificationQueue) : IOrcamentoService
{
    public async Task<OrcamentoDto?> ObterPorOrdemAsync(Guid ordemServicoId, Guid? usuarioId, bool cliente, CancellationToken ct)
    {
        var orcamento = await repository.ObterPorOrdemAsync(ordemServicoId, ct);
        if (orcamento is null) return null;
        if (cliente && (!usuarioId.HasValue || (await clienteRepository.ObterPorUsuarioAsync(usuarioId.Value, ct))?.Id != orcamento.ClienteId)) 
            throw new ConflitoException("O cliente não pode acessar este orçamento.");
        return Map(orcamento);
    }

    public async Task DecidirAsync(Guid ordemServicoId, Guid usuarioId, StatusOrcamento decisao, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        var cliente = await clienteRepository.ObterPorUsuarioAsync(usuarioId, ct) ?? throw new ConflitoException("Usuário não possui cadastro de cliente.");
        var orcamento = await repository.ObterPorOrdemAsync(ordemServicoId, ct) ?? throw new KeyNotFoundException("Orçamento não encontrado.");
        
        if (orcamento.ClienteId != cliente.Id) 
            throw new ConflitoException("O orçamento não pertence ao cliente autenticado.");
        
        orcamento.Decidir(decisao);

        if (decisao == StatusOrcamento.Recusado)
        {
            foreach (var item in orcamento.Itens)
            {
                var itemCatalogo = await itemRepository.ObterPorIdAsync(item.ItemCatalogoId, ct)
                    ?? throw new KeyNotFoundException("Item de catálogo não encontrado para reposição de estoque.");

                itemCatalogo.AdicionarEstoque(item.Quantidade);
                db.HistoricosItensCatalogo.Add(new HistoricoItemCatalogo(itemCatalogo, AcaoHistoricoItemCatalogo.EstoqueAdicionado));
            }
        }
        
        var ordem = await ordemRepository.ObterDetalhadaAsync(ordemServicoId, ct) ?? throw new KeyNotFoundException("Ordem não encontrada.");
        var novoStatus = decisao == StatusOrcamento.Aprovado ? StatusOrdemServico.EmExecucao : StatusOrdemServico.Recebida;
        
        var statusAlterado = ordem.Status == StatusOrdemServico.AguardandoAprovacao;
        if (statusAlterado) ordem.AlterarStatus(novoStatus);
        
        db.HistoricosOrdensServico.Add(new HistoricoOrdemServico(ordem.Id, StatusOrdemServico.AguardandoAprovacao, novoStatus, $"Orçamento {decisao}.", usuarioId, TipoHistoricoOrdem.Orcamento));
        
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        if (statusAlterado)
        {
            var mensagem = $"Olá {ordem.Cliente.Nome}, a ordem de serviço {ordem.Id} foi atualizada para o status {novoStatus}.";
            notificationQueue.EnfileirarEmail(ordem.Cliente.Usuario.Email, "Atualização da ordem de serviço", mensagem);
            if (!string.IsNullOrWhiteSpace(ordem.Cliente.Telefone))
                notificationQueue.EnfileirarSms(ordem.Cliente.Telefone, $"OS {ordem.Id}: status atualizado para {novoStatus}.");
        }
    }

    private static OrcamentoDto Map(Orcamento x) 
        => new(x.Id, x.OrdemServicoId, x.ClienteId, x.ValorMaoDeObra, x.ValorTotal, x.Status, x.DataCriacao, x.DataDecisao, x.Itens.Select(i => new OrcamentoItemDto(i.ItemCatalogoId, i.ItemCatalogo?.Nome ?? string.Empty, i.Quantidade, i.ValorUnitario, i.ValorTotal)).ToList());
}
