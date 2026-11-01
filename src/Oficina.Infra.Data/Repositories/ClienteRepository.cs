using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ClienteRepository(OficinaDbContext db) : BaseRepository<Cliente>(db), IClienteRepository
{
    protected override IQueryable<Cliente> AplicarFiltro(IQueryable<Cliente> query, string? term) 
        => query.Where(x => string.IsNullOrEmpty(term) || x.Nome.ToLower().Contains(term!.ToLower()) || x.Documento.Contains(term!));
    protected override IQueryable<Cliente> AplicarOrdenacao(IQueryable<Cliente> query) 
        => query.OrderBy(x => x.Nome);
}
