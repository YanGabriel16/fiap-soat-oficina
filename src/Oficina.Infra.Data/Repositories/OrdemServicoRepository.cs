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

    public Task<OrdemServico?> ObterDetalhadaAsync(Guid id, CancellationToken ct) => Db.OrdensServico.Include(x => x.Orcamento).ThenInclude(x => x!.Itens).Include(x => x.Itens).SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<OrdemServico>> ListarPorClienteAsync(Guid clienteId, int offset, int limit, CancellationToken ct) => await Db.OrdensServico.AsNoTracking().Include(x => x.Orcamento).Where(x => x.ClienteId == clienteId).OrderByDescending(x => x.DataCriacao).Skip(offset).Take(limit).ToListAsync(ct);
    public async Task<IReadOnlyList<OrdemServico>> ListarPorNomeClienteAsync(string nome, int offset, int limit, CancellationToken ct) => await Db.OrdensServico.AsNoTracking().Include(x => x.Cliente).Include(x => x.Orcamento).Where(x => x.Cliente.Nome.ToLower().Contains(nome.ToLower())).OrderByDescending(x => x.DataCriacao).Skip(offset).Take(limit).ToListAsync(ct);
    public async Task<IReadOnlyList<HistoricoOrdemServico>> ObterHistoricoAsync(Guid ordemServicoId, CancellationToken ct) => await Db.HistoricosOrdensServico.AsNoTracking().Include(x => x.Usuario).Where(x => x.OrdemServicoId == ordemServicoId).OrderBy(x => x.DataCriacao).ToListAsync(ct);
}
