namespace Oficina.Domain.Interfaces.Repositories;
public interface IClienteRepository : IBaseRepository<Entities.Cliente>
{
    Task<Entities.Cliente?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken ct);
}
