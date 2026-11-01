using Oficina.Domain.Common;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public abstract class BaseRepository<TEntity>(OficinaDbContext db) : IBaseRepository<TEntity>
    where TEntity : EntidadeBase
{
    protected OficinaDbContext Db { get; } = db;

    protected virtual IQueryable<TEntity> AplicarFiltro(IQueryable<TEntity> query, string? term) => query;

    protected virtual IQueryable<TEntity> AplicarOrdenacao(IQueryable<TEntity> query) => query.OrderBy(x => x.Id);

    public virtual Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken ct) =>
        Db.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id && !x.Excluida, ct);

    public virtual async Task<(IReadOnlyList<TEntity> Itens, int Total)> ListarAsync(
        string? term, int offset, int limit, CancellationToken ct)
    {
        var query = Db.Set<TEntity>().AsNoTracking().Where(x => !x.Excluida);
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
