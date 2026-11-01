using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;

namespace Oficina.Domain.Interfaces.Services;

public interface IOrcamentoService
{
    Task<OrcamentoDto?> ObterPorOrdemAsync(Guid ordemServicoId, Guid? usuarioId, bool cliente, CancellationToken ct);
    Task DecidirAsync(Guid ordemServicoId, Guid usuarioId, StatusOrcamento decisao, CancellationToken ct);
}
