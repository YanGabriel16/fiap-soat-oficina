using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Interfaces.Services;

public interface IOrdemServicoService : IBaseService<OrdemServicoDto>
{
    Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct);
    Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct);
}
