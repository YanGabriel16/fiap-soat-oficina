using Oficina.Domain.Dtos;

namespace Oficina.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<Guid> CadastrarAsync(string nome, string email, string senha, CancellationToken ct);
    Task<LoginDto?> LoginAsync(string email, string senha, CancellationToken ct);
    Task<Guid> CadastrarAsync(string nome, string email, string senha, Enums.RoleUsuario role, string? documento, CancellationToken ct);
}
