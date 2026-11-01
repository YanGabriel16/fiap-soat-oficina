using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct);
    Task AdicionarAsync(Usuario usuario, CancellationToken ct);
    Task<bool> ExistePorEmailAsync(string email, CancellationToken ct);
}
