using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class OrdemServicoRepository(OficinaDbContext db) : BaseRepository<OrdemServico>(db), IOrdemServicoRepository
{
    protected override IQueryable<OrdemServico> AplicarFiltro(IQueryable<OrdemServico> query, string? term) 
        => from ordem in query
           where string.IsNullOrEmpty(term) 
            || EF.Functions.Like(ordem.Descricao.ToLower(), $"%{term!.ToLower()}%")
           select ordem;
    protected override IQueryable<OrdemServico> AplicarOrdenacao(IQueryable<OrdemServico> query) 
        => from ordem in query
           orderby ordem.DataCriacao descending
           select ordem;

    public Task<OrdemServico?> ObterDetalhadaAsync(Guid id, CancellationToken ct) 
        => (from ordem in Db.OrdensServico
                .Include(x => x.Orcamento)
                    .ThenInclude(x => x!.Itens)
                .Include(x => x.Itens)
                    .ThenInclude(x => x.ItemCatalogo)
                .Include(x => x.Cliente)
                    .ThenInclude(x => x.Usuario)
            where ordem.Id == id
            select ordem).SingleOrDefaultAsync(ct);

    public async Task<IReadOnlyList<OrdemServico>> ListarPorClienteAsync(Guid clienteId, int offset, int limit, CancellationToken ct) 
        => await (from ordem in Db.OrdensServico
                        .AsNoTracking()
                        .Include(x => x.Orcamento)
                  where ordem.ClienteId == clienteId
                  orderby ordem.DataCriacao descending
                  select ordem)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    
    public async Task<IReadOnlyList<OrdemServico>> ListarPorNomeClienteAsync(string nome, int offset, int limit, CancellationToken ct) 
        => await (from ordem in Db.OrdensServico
                        .AsNoTracking()
                        .Include(x => x.Cliente)
                        .Include(x => x.Orcamento)
                  where EF.Functions.Like(ordem.Cliente.Nome.ToLower(), $"%{nome.ToLower()}%")
                  orderby ordem.DataCriacao descending
                  select ordem)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    
    public async Task<IReadOnlyList<HistoricoOrdemServico>> ObterHistoricoAsync(Guid ordemServicoId, CancellationToken ct) 
        => await (from historico in Db.HistoricosOrdensServico
                            .AsNoTracking()
                            .Include(x => x.Usuario)
                  where historico.OrdemServicoId == ordemServicoId
                  orderby historico.DataCriacao
                  select historico)
            .ToListAsync(ct);
}
