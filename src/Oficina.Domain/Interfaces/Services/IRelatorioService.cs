using Oficina.Domain.Dtos;

namespace Oficina.Domain.Interfaces.Services;

public interface IRelatorioService
{
    Task<RelatorioTempoOrdensResponse> ObterTempoOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct);
    Task<RelatorioOrcamentosResponse> ObterOrcamentosAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct);
    Task<IReadOnlyList<RelatorioStatusOrdemDto>> ObterStatusOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct);
    Task<IReadOnlyList<RelatorioEstoqueDto>> ObterEstoqueAsync(int limite, CancellationToken ct);
}
