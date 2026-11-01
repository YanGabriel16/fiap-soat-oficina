using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class OrdemServicoRepository(OficinaDbContext db) : BaseRepository<OrdemServico>(db), IOrdemServicoRepository
{
    protected override IQueryable<OrdemServico> AplicarFiltro(IQueryable<OrdemServico> query, string? term) 
        => query.Where(x => string.IsNullOrEmpty(term) || x.Id.ToString().Contains(term!));
    protected override IQueryable<OrdemServico> AplicarOrdenacao(IQueryable<OrdemServico> query) 
        => query.OrderByDescending(x => x.DataCriacao);
}
