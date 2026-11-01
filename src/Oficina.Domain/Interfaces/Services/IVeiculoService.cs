using Oficina.Domain.Dtos;
namespace Oficina.Domain.Interfaces.Services;
public interface IVeiculoService { Task<ResultadoPaginadoDto<VeiculoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task<VeiculoDto?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<Guid> CriarAsync(Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct); Task<Guid> AtualizarAsync(Guid id, Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct); Task<bool> ExcluirAsync(Guid id, CancellationToken ct); }
