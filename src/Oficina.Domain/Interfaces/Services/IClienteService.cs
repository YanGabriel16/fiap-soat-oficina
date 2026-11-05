using Oficina.Domain.Dtos;
namespace Oficina.Domain.Interfaces.Services;

public interface IClienteService : IBaseService<ClienteDto>
{
    Task<Guid> CriarAsync(string nome, string documento, Guid? usuarioId, string? telefone, CancellationToken ct);
    Task<Guid> AtualizarAsync(Guid id, string nome, string documento, string? telefone, CancellationToken ct);
}
