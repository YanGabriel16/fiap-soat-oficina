using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class VeiculoRepository(OficinaDbContext db) : BaseRepository<Veiculo>(db), IVeiculoRepository
{
    protected override IQueryable<Veiculo> AplicarFiltro(IQueryable<Veiculo> query, string? term) 
        => from veiculo in query
           where string.IsNullOrEmpty(term)
                 || EF.Functions.Like(veiculo.Placa, $"%{term!}%")
                 || EF.Functions.Like(veiculo.Marca.ToLower(), $"%{term!.ToLower()}%")
                 || EF.Functions.Like(veiculo.Modelo.ToLower(), $"%{term!.ToLower()}%")
           select veiculo;
    protected override IQueryable<Veiculo> AplicarOrdenacao(IQueryable<Veiculo> query) 
        => from veiculo in query
           orderby veiculo.Placa
           select veiculo;
}
