using Oficina.Domain.Common;

namespace Oficina.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntidadeBase
{
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<(IReadOnlyList<TEntity> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct);
    Task AdicionarAsync(TEntity entidade, CancellationToken ct);
    Task AtualizarAsync(TEntity entidade, CancellationToken ct);
    Task ExcluirAsync(TEntity entidade, CancellationToken ct);
}
