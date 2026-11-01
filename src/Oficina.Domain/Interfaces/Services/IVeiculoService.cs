using Oficina.Domain.Dtos;
namespace Oficina.Domain.Interfaces.Services;

public interface IVeiculoService : IBaseService<VeiculoDto>
{
    Task<Guid> CriarAsync(Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct);
    Task<Guid> AtualizarAsync(Guid id, Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct);
}
