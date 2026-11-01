using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class VeiculoRepository(OficinaDbContext db) : BaseRepository<Veiculo>(db), IVeiculoRepository
{
    protected override IQueryable<Veiculo> AplicarFiltro(IQueryable<Veiculo> query, string? term) 
        => query.Where(x => string.IsNullOrEmpty(term) || x.Placa.Contains(term!) || x.Marca.ToLower().Contains(term!.ToLower()) || x.Modelo.ToLower().Contains(term!.ToLower()));
    protected override IQueryable<Veiculo> AplicarOrdenacao(IQueryable<Veiculo> query) 
        => query.OrderBy(x => x.Placa);
}
