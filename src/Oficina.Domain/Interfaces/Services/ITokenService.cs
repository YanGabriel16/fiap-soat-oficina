using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GerarAsync(Usuario usuario, CancellationToken ct);
}
