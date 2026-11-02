using Oficina.Domain.Common;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public abstract class BaseRepository<TEntity>(OficinaDbContext db) : IBaseRepository<TEntity>
    where TEntity : EntidadeBase
{
    protected OficinaDbContext Db { get; } = db;

    protected virtual IQueryable<TEntity> AplicarFiltro(IQueryable<TEntity> query, string? term) 
        => from entidade in query
           select entidade;

    protected virtual IQueryable<TEntity> AplicarOrdenacao(IQueryable<TEntity> query) 
        => from entidade in query
           orderby entidade.Id
           select entidade;

    public virtual Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken ct) 
        => (from entidade in Db.Set<TEntity>()
            where entidade.Id == id && entidade.DataExclusao == null
            select entidade).SingleOrDefaultAsync(ct);

    public virtual async Task<(IReadOnlyList<TEntity> Itens, int Total)> ListarAsync(
        string? term, int offset, int limit, CancellationToken ct)
    {
        var query = from entidade in Db.Set<TEntity>().AsNoTracking()
                    where entidade.DataExclusao == null
                    select entidade;
        
        query = AplicarFiltro(query, term);
        var total = await query.CountAsync(ct);
        var itens = await AplicarOrdenacao(query).Skip(offset).Take(limit).ToListAsync(ct);
        
        return (itens, total);
    }

    public virtual async Task AdicionarAsync(TEntity entidade, CancellationToken ct)
    {
        await Db.Set<TEntity>().AddAsync(entidade, ct);
        await Db.SaveChangesAsync(ct);
    }

    public virtual async Task AtualizarAsync(TEntity entidade, CancellationToken ct)
    {
        Db.Set<TEntity>().Update(entidade);
        await Db.SaveChangesAsync(ct);
    }

    public virtual async Task ExcluirAsync(TEntity entidade, CancellationToken ct)
    {
        Db.Set<TEntity>().Update(entidade);
        await Db.SaveChangesAsync(ct);
    }
}
