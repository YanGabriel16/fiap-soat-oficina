using Oficina.Domain.Common;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public abstract class BaseService<TEntity, TDto, TRepository>(TRepository repository) : IBaseService<TDto>
    where TEntity : EntidadeBase
    where TRepository : IBaseRepository<TEntity>
{
    protected TRepository Repository { get; } = repository;

    protected abstract TDto Map(TEntity entidade);

    public async Task<ResultadoPaginadoDto<TDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct)
    {
        (offset, limit) = Normalizar(offset, limit);
        var resultado = await Repository.ListarAsync(term, offset, limit, ct);
        return new(resultado.Itens.Select(Map).ToList(), offset, limit, resultado.Total);
    }

    public virtual async Task<TDto?> ObterPorIdAsync(Guid id, CancellationToken ct) 
        => (await Repository.ObterPorIdAsync(id, ct)) is { } entidade ? Map(entidade) : default;

    public async Task<bool> ExcluirAsync(Guid id, CancellationToken ct)
    {
        var entidade = await Repository.ObterPorIdAsync(id, ct);
        
        if (entidade is null)
            return false;

        entidade.Excluir();
        await Repository.ExcluirAsync(entidade, ct);
        
        return true;
    }

    private static (int Offset, int Limit) Normalizar(int offset, int limit) 
        => (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
}
