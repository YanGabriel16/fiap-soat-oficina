namespace Oficina.Domain.Interfaces.Repositories;
public interface IOrdemServicoRepository : IBaseRepository<Entities.OrdemServico>
{
    Task<Entities.OrdemServico?> ObterDetalhadaAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Entities.OrdemServico>> ListarPorClienteAsync(Guid clienteId, int offset, int limit, CancellationToken ct);
    Task<IReadOnlyList<Entities.OrdemServico>> ListarPorNomeClienteAsync(string nome, int offset, int limit, CancellationToken ct);
    Task<IReadOnlyList<Entities.HistoricoOrdemServico>> ObterHistoricoAsync(Guid ordemServicoId, CancellationToken ct);
}
