using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Interfaces.Services;
public interface IItemCatalogoService { Task<ResultadoPaginadoDto<ItemCatalogoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task<ItemCatalogoDto?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<Guid> CriarAsync(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct); Task<Guid> AtualizarAsync(Guid id, string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct); Task<bool> ExcluirAsync(Guid id, CancellationToken ct); }
