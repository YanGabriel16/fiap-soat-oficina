using Oficina.Domain.Dtos;
namespace Oficina.Domain.Interfaces.Services;
public interface IClienteService { Task<ResultadoPaginadoDto<ClienteDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct); Task<ClienteDto?> ObterPorIdAsync(Guid id, CancellationToken ct); Task<Guid> CriarAsync(string nome, string documento, CancellationToken ct); Task<Guid> AtualizarAsync(Guid id, string nome, string documento, CancellationToken ct); Task<bool> ExcluirAsync(Guid id, CancellationToken ct); }
