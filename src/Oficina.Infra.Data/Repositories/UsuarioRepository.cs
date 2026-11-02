using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public sealed class UsuarioRepository(OficinaDbContext context) : IUsuarioRepository
{
    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct) 
        => (from usuario in context.Usuarios
            where usuario.Email == email
            select usuario).SingleOrDefaultAsync(ct);

    public async Task AdicionarAsync(Usuario usuario, CancellationToken ct)
    {
        await context.Usuarios.AddAsync(usuario, ct);
        await context.SaveChangesAsync(ct);
    }

    public Task<bool> ExistePorEmailAsync(string email, CancellationToken ct) 
        => (from usuario in context.Usuarios
            where usuario.Email == email
            select usuario).AnyAsync(ct);
}
