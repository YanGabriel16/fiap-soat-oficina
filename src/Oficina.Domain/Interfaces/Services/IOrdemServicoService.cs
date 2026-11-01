using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Interfaces.Services;
public interface IOrdemServicoService { Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task<OrdemServicoDto?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct); Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct); Task<bool> ExcluirAsync(Guid id, CancellationToken ct); }
