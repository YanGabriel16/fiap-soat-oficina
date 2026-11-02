using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ItemCatalogoRepository(OficinaDbContext db) : BaseRepository<ItemCatalogo>(db), IItemCatalogoRepository
{
    protected override IQueryable<ItemCatalogo> AplicarFiltro(IQueryable<ItemCatalogo> query, string? term) 
        => from item in query
           where string.IsNullOrEmpty(term) 
            || EF.Functions.Like(item.Nome.ToLower(), $"%{term!.ToLower()}%")
           select item;
    protected override IQueryable<ItemCatalogo> AplicarOrdenacao(IQueryable<ItemCatalogo> query) 
        => from item in query
           orderby item.Nome
           select item;
}
