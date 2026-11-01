using Oficina.Domain.Dtos;

namespace Oficina.Domain.Interfaces.Services;

public interface IBaseService<TDto>
{
    Task<ResultadoPaginadoDto<TDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct);
    Task<TDto?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExcluirAsync(Guid id, CancellationToken ct);
}
