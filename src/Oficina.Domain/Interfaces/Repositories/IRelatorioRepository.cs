using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Repositories;

public interface IRelatorioRepository
{
    Task<IReadOnlyList<OrdemServico>> ListarOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct);
    Task<IReadOnlyList<Orcamento>> ListarOrcamentosAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct);
    Task<IReadOnlyList<ItemCatalogo>> ListarEstoqueAsync(int limite, CancellationToken ct);
}
