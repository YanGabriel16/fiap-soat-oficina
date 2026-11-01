using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;
public sealed class OrdemServicoService(IOrdemServicoRepository repository) : IOrdemServicoService
{
    public async Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { (offset, limit) = Normalizar(offset, limit); var r = await repository.ListarAsync(term, offset, limit, ct); return new(r.Itens.Select(Map).ToList(), offset, limit, r.Total); }
    public async Task<OrdemServicoDto?> ObterPorIdAsync(Guid id, CancellationToken ct) => (await repository.ObterPorIdAsync(id, ct)) is { } e ? Map(e) : null;
    public async Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct) { var e = new OrdemServico(clienteId, veiculoId); await repository.AdicionarAsync(e, ct); return e.Id; }
    public async Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Ordem de serviço não encontrada."); e.Atualizar(clienteId, veiculoId, status, valorTotal); await repository.AtualizarAsync(e, ct); return e.Id; }
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct); if (e is null) return false; e.Excluir(); await repository.ExcluirAsync(e, ct); return true; }
    private static OrdemServicoDto Map(OrdemServico e) => new(e.Id, e.ClienteId, e.VeiculoId, e.Status, e.ValorTotal, e.DataCriacao, e.DataUltimaModificacao);
    private static (int, int) Normalizar(int offset, int limit) => (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
}
