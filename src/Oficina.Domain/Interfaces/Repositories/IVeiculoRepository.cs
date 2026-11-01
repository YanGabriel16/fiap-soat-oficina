using Oficina.Domain.Entities;
namespace Oficina.Domain.Interfaces.Repositories;
public interface IVeiculoRepository { Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<(IReadOnlyList<Veiculo> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task AdicionarAsync(Veiculo veiculo, CancellationToken ct); Task AtualizarAsync(Veiculo veiculo, CancellationToken ct); Task ExcluirAsync(Veiculo veiculo, CancellationToken ct); }
