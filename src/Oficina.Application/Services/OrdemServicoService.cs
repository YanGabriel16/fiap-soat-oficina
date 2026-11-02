using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class OrdemServicoService(IOrdemServicoRepository repository, IItemCatalogoRepository itemRepository, IClienteRepository clienteRepository, Oficina.Infra.Data.Context.OficinaDbContext db) : BaseService<OrdemServico, OrdemServicoDto, IOrdemServicoRepository>(repository), IOrdemServicoService
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
        
        e.Atualizar(clienteId, veiculoId, status, valorTotal);
        await Repository.AtualizarAsync(e, ct);
        
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
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        var ordem = new OrdemServico(clienteId, veiculoId, atendenteId, descricao);
        
        decimal total = valorMaoDeObra;
        
        foreach (var item in itens)
        {
            var catalogo = await itemRepository.ObterPorIdAsync(item.ItemCatalogoId, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");
            
            if (item.Quantidade > catalogo.QuantidadeEstoque) 
                throw new ArgumentException("Estoque insuficiente.");
            
            ordem.Itens.Add(new OrdemServicoItem(ordem.Id, catalogo.Id, item.Quantidade, catalogo.Preco)); total += item.Quantidade * catalogo.Preco;
        }
        
        ordem.DefinirOrcamento(valorMaoDeObra, total);
        db.OrdensServico.Add(ordem);
        
        var orcamento = new Orcamento(ordem.Id, clienteId, valorMaoDeObra);
        
        foreach (var item in ordem.Itens) 
            
            orcamento.AdicionarItem(item.ItemCatalogoId, item.Quantidade, item.ValorUnitario);
        db.Orcamentos.Add(orcamento);
        db.HistoricosOrdensServico.Add(new HistoricoOrdemServico(ordem.Id, null, ordem.Status, "Ordem criada.", atendenteId));
        
        await db.SaveChangesAsync(ct); await transaction.CommitAsync(ct); return ordem.Id;
    }

    public async Task AtualizarStatusAsync(Guid id, StatusOrdemServico novoStatus, Guid usuarioId, string? observacao, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        var ordem = await Repository.ObterDetalhadaAsync(id, ct) ?? throw new KeyNotFoundException("Ordem de serviço não encontrada.");
        var anterior = ordem.Status; ordem.AlterarStatus(novoStatus);
        
        db.HistoricosOrdensServico.Add(new HistoricoOrdemServico(id, anterior, novoStatus, observacao, usuarioId));
        
        await db.SaveChangesAsync(ct); 
        await transaction.CommitAsync(ct);
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
}
