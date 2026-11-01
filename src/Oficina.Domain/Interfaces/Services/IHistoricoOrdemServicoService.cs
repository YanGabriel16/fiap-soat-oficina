using Oficina.Domain.Dtos;

namespace Oficina.Domain.Interfaces.Services;

public interface IHistoricoOrdemServicoService
{
    Task<IReadOnlyList<HistoricoOrdemServicoDto>> ListarAsync(Guid ordemServicoId, Guid? usuarioId, bool cliente, CancellationToken ct);
}
