using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Repositories;

public interface IHistoricoItemCatalogoRepository
{
    Task AdicionarAsync(HistoricoItemCatalogo historico, CancellationToken ct);
    Task<IReadOnlyList<HistoricoItemCatalogo>> ListarPorItemAsync(Guid itemCatalogoId, CancellationToken ct);
}
