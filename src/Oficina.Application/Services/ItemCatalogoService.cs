using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class ItemCatalogoService(IItemCatalogoRepository repository) : BaseService<ItemCatalogo, ItemCatalogoDto, IItemCatalogoRepository>(repository), IItemCatalogoService
{
    public async Task<Guid> CriarAsync(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct)
    {
        var e = new ItemCatalogo(nome, preco, tipo, quantidadeEstoque);
        await Repository.AdicionarAsync(e, ct);
        
        return e.Id;
    }
    public async Task<Guid> AtualizarAsync(Guid id, string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado.");
        
        e.Atualizar(nome, preco, tipo, quantidadeEstoque);
        await Repository.AtualizarAsync(e, ct);
        
        return e.Id;
    }
    protected override ItemCatalogoDto Map(ItemCatalogo e)
        => new(e.Id, e.Nome, e.Preco, e.Tipo, e.QuantidadeEstoque, e.DataCriacao, e.DataUltimaModificacao);
}
