using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ItemCatalogoRepository(OficinaDbContext db) : BaseRepository<ItemCatalogo>(db), IItemCatalogoRepository
{
    protected override IQueryable<ItemCatalogo> AplicarFiltro(IQueryable<ItemCatalogo> query, string? term) 
        => query.Where(x => string.IsNullOrEmpty(term) || x.Nome.ToLower().Contains(term!.ToLower()));
    protected override IQueryable<ItemCatalogo> AplicarOrdenacao(IQueryable<ItemCatalogo> query) 
        => query.OrderBy(x => x.Nome);
}
