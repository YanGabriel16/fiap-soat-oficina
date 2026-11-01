using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public sealed class TokenRepository(OficinaDbContext context) : ITokenRepository
{
    public async Task AdicionarAsync(Token token, CancellationToken ct)
    {
        await context.Tokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }
}
