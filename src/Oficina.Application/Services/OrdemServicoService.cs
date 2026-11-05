using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class OrdemServicoService(
    IOrdemServicoRepository repository,
    IItemCatalogoRepository itemRepository,
    IClienteRepository clienteRepository,
    Oficina.Infra.Data.Context.OficinaDbContext db,
    INotificationQueue notificationQueue) : BaseService<OrdemServico, OrdemServicoDto, IOrdemServicoRepository>(repository), IOrdemServicoService
{
    public async Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct)
    {
        var e = new OrdemServico(clienteId, veiculoId);
        
        await Repository.AdicionarAsync(e, ct);
        
        return e.Id;
    }
    public async Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Ordem de serviço não encontrada.");
        var statusAlterado = e.Status != status;
        var cliente = statusAlterado
            ? await clienteRepository.ObterDetalhadoAsync(clienteId, ct) ?? throw new KeyNotFoundException("Cliente não encontrado.")
            : null;
        
        e.Atualizar(clienteId, veiculoId, status, valorTotal);
        await Repository.AtualizarAsync(e, ct);

        if (cliente is not null)
            EnfileirarNotificacaoStatus(cliente, id, status);
        
        return e.Id;
    }

    public override async Task<OrdemServicoDto?> ObterPorIdAsync(Guid id, CancellationToken ct)
        => (await Repository.ObterDetalhadaAsync(id, ct)) is { } ordem ? Map(ordem) : null;

    protected override OrdemServicoDto Map(OrdemServico e)
        => new(
            e.Id,
            e.ClienteId,
            e.VeiculoId,
            e.Status,
            e.ValorTotal,
            e.DataCriacao,
            e.DataUltimaModificacao,
            e.Itens.Select(x => new OrdemServicoItemDto(
                x.ItemCatalogoId,
                x.ItemCatalogo?.Nome ?? string.Empty,
                x.Quantidade,
                x.ValorUnitario,
                x.ValorTotal)).ToList());

    public async Task<Guid> CriarComOrcamentoAsync(Guid clienteId, Guid veiculoId, Guid atendenteId, string descricao, IReadOnlyList<(Guid ItemCatalogoId, int Quantidade)> itens, decimal valorMaoDeObra, CancellationToken ct)
    {
        var cliente = await clienteRepository.ObterDetalhadoAsync(clienteId, ct)
            ?? throw new KeyNotFoundException("Cliente não encontrado.");

        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        var ordem = new OrdemServico(clienteId, veiculoId, atendenteId, descricao);
        
        decimal total = valorMaoDeObra;
        
        foreach (var item in itens)
        {
            var catalogo = await itemRepository.ObterPorIdAsync(item.ItemCatalogoId, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");
            
            if (item.Quantidade > catalogo.QuantidadeEstoque) 
                throw new ArgumentException("Estoque insuficiente.");
            
            catalogo.RemoverEstoque(item.Quantidade);
            db.HistoricosItensCatalogo.Add(new HistoricoItemCatalogo(catalogo, AcaoHistoricoItemCatalogo.EstoqueRemovido));
            ordem.Itens.Add(new OrdemServicoItem(ordem.Id, catalogo.Id, item.Quantidade, catalogo.Preco)); total += item.Quantidade * catalogo.Preco;
        }
        
        ordem.DefinirOrcamento(valorMaoDeObra, total);
        db.OrdensServico.Add(ordem);
        
        var orcamento = new Orcamento(ordem.Id, clienteId, valorMaoDeObra);
        
        foreach (var item in ordem.Itens) 
            
            orcamento.AdicionarItem(item.ItemCatalogoId, item.Quantidade, item.ValorUnitario);
        db.Orcamentos.Add(orcamento);
        db.HistoricosOrdensServico.Add(new HistoricoOrdemServico(ordem.Id, null, ordem.Status, "Ordem criada.", atendenteId));
        
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        var mensagem = $"Olá {cliente.Nome}, o orçamento da ordem de serviço {ordem.Id} foi criado. Valor total: {total:C}. Consulte os detalhes no sistema.";
        notificationQueue.EnfileirarEmail(cliente.Usuario.Email, "Orçamento da ordem de serviço", mensagem);
        if (!string.IsNullOrWhiteSpace(cliente.Telefone))
            notificationQueue.EnfileirarSms(cliente.Telefone, $"Orçamento da OS {ordem.Id}: {total:C}. Consulte os detalhes no sistema.");

        return ordem.Id;
    }

    public async Task AtualizarStatusAsync(Guid id, StatusOrdemServico novoStatus, Guid usuarioId, string? observacao, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        var ordem = await Repository.ObterDetalhadaAsync(id, ct) ?? throw new KeyNotFoundException("Ordem de serviço não encontrada.");
        var anterior = ordem.Status; ordem.AlterarStatus(novoStatus);
        
        db.HistoricosOrdensServico.Add(new HistoricoOrdemServico(id, anterior, novoStatus, observacao, usuarioId));
        
        await db.SaveChangesAsync(ct); 
        await transaction.CommitAsync(ct);

        EnfileirarNotificacaoStatus(ordem.Cliente, ordem.Id, novoStatus);
    }

    public async Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarDoClienteAsync(Guid usuarioId, int offset, int limit, CancellationToken ct)
    {
        (offset, limit) = (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
        var cliente = await clienteRepository.ObterPorUsuarioAsync(usuarioId, ct)
            ?? throw new KeyNotFoundException("Cliente não encontrado para o usuário autenticado.");
        var itens = await Repository.ListarPorClienteAsync(cliente.Id, offset, limit, ct); 
        
        return new(itens.Select(Map).ToList(), offset, limit, itens.Count);
    }
    public async Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarPorNomeClienteAsync(string nome, int offset, int limit, CancellationToken ct)
    {
        (offset, limit) = (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
        var itens = await Repository.ListarPorNomeClienteAsync(nome, offset, limit, ct); 
        return new(itens.Select(Map).ToList(), offset, limit, itens.Count);
    }

    private void EnfileirarNotificacaoStatus(Cliente cliente, Guid ordemServicoId, StatusOrdemServico status)
    {
        var mensagem = $"Olá {cliente.Nome}, a ordem de serviço {ordemServicoId} foi atualizada para o status {status}.";
        notificationQueue.EnfileirarEmail(cliente.Usuario.Email, "Atualização da ordem de serviço", mensagem);
        if (!string.IsNullOrWhiteSpace(cliente.Telefone))
            notificationQueue.EnfileirarSms(cliente.Telefone, $"OS {ordemServicoId}: status atualizado para {status}.");
    }
}
