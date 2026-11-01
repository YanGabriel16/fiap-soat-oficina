using Oficina.Domain.Dtos;
using Oficina.Domain.Common;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;
public sealed class ClienteService(IClienteRepository repository) : IClienteService
{
    public async Task<ResultadoPaginadoDto<ClienteDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { (offset, limit) = Normalizar(offset, limit); var r = await repository.ListarAsync(term, offset, limit, ct); return new(r.Itens.Select(Map).ToList(), offset, limit, r.Total); }
    public async Task<ClienteDto?> ObterPorIdAsync(Guid id, CancellationToken ct) => (await repository.ObterPorIdAsync(id, ct)) is { } e ? Map(e) : null;
    public async Task<Guid> CriarAsync(string nome, string documento, CancellationToken ct) { ValidarDocumento(documento); var e = new Cliente(nome, documento); await repository.AdicionarAsync(e, ct); return e.Id; }
    public async Task<Guid> AtualizarAsync(Guid id, string nome, string documento, CancellationToken ct) { ValidarDocumento(documento); var e = await repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Cliente não encontrado."); e.Atualizar(nome, documento); await repository.AtualizarAsync(e, ct); return e.Id; }
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct); if (e is null) return false; e.Excluir(); await repository.ExcluirAsync(e, ct); return true; }
    private static ClienteDto Map(Cliente e) => new(e.Id, e.Nome, e.Documento, e.DataCriacao, e.DataUltimaModificacao);
    private static (int, int) Normalizar(int offset, int limit) => (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
    private static void ValidarDocumento(string documento) { if (!Validadores.Documento(documento)) throw new ArgumentException("CPF ou CNPJ inválido."); }
}
