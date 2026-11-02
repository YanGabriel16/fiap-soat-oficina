using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class ItemCatalogoService(IItemCatalogoRepository repository, IHistoricoItemCatalogoRepository historicoRepository) : BaseService<ItemCatalogo, ItemCatalogoDto, IItemCatalogoRepository>(repository), IItemCatalogoService
{
    public async Task<Guid> CriarAsync(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct)
    {
        var e = new ItemCatalogo(nome, preco, tipo, quantidadeEstoque);
        await Repository.AdicionarAsync(e, ct);
        await RegistrarHistoricoAsync(e, AcaoHistoricoItemCatalogo.Criado, ct);
        
        return e.Id;
    }
    
    public async Task<Guid> AtualizarAsync(Guid id, string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");
        
        e.Atualizar(nome, preco, tipo, quantidadeEstoque);
        await Repository.AtualizarAsync(e, ct);
        await RegistrarHistoricoAsync(e, AcaoHistoricoItemCatalogo.Atualizado, ct);
        
        return e.Id;
    }

    public async Task<Guid> AdicionarEstoqueAsync(Guid id, int quantidade, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");

        e.AdicionarEstoque(quantidade);
        await Repository.AtualizarAsync(e, ct);
        await RegistrarHistoricoAsync(e, AcaoHistoricoItemCatalogo.EstoqueAdicionado, ct);

        return e.Id;
    }

    public async Task<IReadOnlyList<HistoricoItemCatalogoDto>> ListarHistoricoAsync(Guid id, CancellationToken ct)
    {
        _ = await Repository.ObterPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");

        var historicos = await historicoRepository.ListarPorItemAsync(id, ct);
        return historicos.Select(x => new HistoricoItemCatalogoDto(
            x.Id,
            x.ItemCatalogoId,
            x.Acao,
            x.Nome,
            x.Preco,
            x.Tipo,
            x.QuantidadeEstoque,
            x.DataEvento)).ToList();
    }

    public override async Task<bool> ExcluirAsync(Guid id, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct);
        if (e is null) return false;

        e.Excluir();
        await Repository.ExcluirAsync(e, ct);
        await RegistrarHistoricoAsync(e, AcaoHistoricoItemCatalogo.Excluido, ct);
        return true;
    }

    private Task RegistrarHistoricoAsync(ItemCatalogo item, AcaoHistoricoItemCatalogo acao, CancellationToken ct)
        => historicoRepository.AdicionarAsync(new HistoricoItemCatalogo(item, acao), ct);

    protected override ItemCatalogoDto Map(ItemCatalogo e)
        => new(e.Id, e.Nome, e.Preco, e.Tipo, e.QuantidadeEstoque, e.DataCriacao, e.DataUltimaModificacao);
}
