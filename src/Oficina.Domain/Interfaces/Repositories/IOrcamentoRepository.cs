using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Repositories;

public interface IOrcamentoRepository
{
    Task<Orcamento?> ObterDetalhadoAsync(Guid id, CancellationToken ct);
    Task<Orcamento?> ObterPorOrdemAsync(Guid ordemServicoId, CancellationToken ct);
    Task AdicionarAsync(Orcamento orcamento, CancellationToken ct);
    Task AtualizarAsync(Orcamento orcamento, CancellationToken ct);
}
