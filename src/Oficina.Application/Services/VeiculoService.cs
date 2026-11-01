using Oficina.Domain.Dtos;
using Oficina.Domain.Common;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;
public sealed class VeiculoService(IVeiculoRepository repository) : IVeiculoService
{
    public async Task<ResultadoPaginadoDto<VeiculoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { (offset, limit) = Normalizar(offset, limit); var r = await repository.ListarAsync(term, offset, limit, ct); return new(r.Itens.Select(Map).ToList(), offset, limit, r.Total); }
    public async Task<VeiculoDto?> ObterPorIdAsync(Guid id, CancellationToken ct) => (await repository.ObterPorIdAsync(id, ct)) is { } e ? Map(e) : null;
    public async Task<Guid> CriarAsync(Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct) { ValidarPlaca(placa); var e = new Veiculo(clienteId, placa.ToUpperInvariant(), marca, modelo, ano); await repository.AdicionarAsync(e, ct); return e.Id; }
    public async Task<Guid> AtualizarAsync(Guid id, Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct) { ValidarPlaca(placa); var e = await repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Veículo não encontrado."); e.Atualizar(clienteId, placa.ToUpperInvariant(), marca, modelo, ano); await repository.AtualizarAsync(e, ct); return e.Id; }
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct); if (e is null) return false; e.Excluir(); await repository.ExcluirAsync(e, ct); return true; }
    private static VeiculoDto Map(Veiculo e) => new(e.Id, e.ClienteId, e.Placa, e.Marca, e.Modelo, e.Ano, e.DataCriacao, e.DataUltimaModificacao);
    private static (int, int) Normalizar(int offset, int limit) => (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
    private static void ValidarPlaca(string placa) { if (!Validadores.Placa(placa)) throw new ArgumentException("Placa de veículo inválida."); }
}
