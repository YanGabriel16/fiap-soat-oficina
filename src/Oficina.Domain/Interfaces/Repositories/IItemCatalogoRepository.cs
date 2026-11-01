using Oficina.Domain.Entities;
namespace Oficina.Domain.Interfaces.Repositories;
public interface IItemCatalogoRepository { Task<ItemCatalogo?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<(IReadOnlyList<ItemCatalogo> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task AdicionarAsync(ItemCatalogo item, CancellationToken ct); Task AtualizarAsync(ItemCatalogo item, CancellationToken ct); Task ExcluirAsync(ItemCatalogo item, CancellationToken ct); }
