using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Repositories;

public interface ITokenRepository
{
    Task AdicionarAsync(Token token, CancellationToken ct);
}
