using Oficina.Domain.Entities;
namespace Oficina.Domain.Interfaces.Repositories;
public interface IOrdemServicoRepository { Task<OrdemServico?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<(IReadOnlyList<OrdemServico> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task AdicionarAsync(OrdemServico ordem, CancellationToken ct); Task AtualizarAsync(OrdemServico ordem, CancellationToken ct); Task ExcluirAsync(OrdemServico ordem, CancellationToken ct); }
