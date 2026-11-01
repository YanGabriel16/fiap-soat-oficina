using Oficina.Domain.Entities;
namespace Oficina.Domain.Interfaces.Repositories;
public interface IClienteRepository { Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<(IReadOnlyList<Cliente> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task AdicionarAsync(Cliente cliente, CancellationToken ct); Task AtualizarAsync(Cliente cliente, CancellationToken ct); Task ExcluirAsync(Cliente cliente, CancellationToken ct); }
