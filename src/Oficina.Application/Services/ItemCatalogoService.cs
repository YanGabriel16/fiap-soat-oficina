using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;
public sealed class ItemCatalogoService(IItemCatalogoRepository repository) : IItemCatalogoService
{
    public async Task<ResultadoPaginadoDto<ItemCatalogoDto>> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { (offset, limit) = Normalizar(offset, limit); var r = await repository.ListarAsync(term, offset, limit, ct); return new(r.Itens.Select(Map).ToList(), offset, limit, r.Total); }
    public async Task<ItemCatalogoDto?> ObterPorIdAsync(Guid id, CancellationToken ct) => (await repository.ObterPorIdAsync(id, ct)) is { } e ? Map(e) : null;
    public async Task<Guid> CriarAsync(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct) { var e = new ItemCatalogo(nome, preco, tipo, quantidadeEstoque); await repository.AdicionarAsync(e, ct); return e.Id; }
    public async Task<Guid> AtualizarAsync(Guid id, string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Item de catálogo não encontrado."); e.Atualizar(nome, preco, tipo, quantidadeEstoque); await repository.AtualizarAsync(e, ct); return e.Id; }
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken ct) { var e = await repository.ObterPorIdAsync(id, ct); if (e is null) return false; e.Excluir(); await repository.ExcluirAsync(e, ct); return true; }
    private static ItemCatalogoDto Map(ItemCatalogo e) => new(e.Id, e.Nome, e.Preco, e.Tipo, e.QuantidadeEstoque, e.DataCriacao, e.DataUltimaModificacao);
    private static (int, int) Normalizar(int offset, int limit) => (Math.Max(0, offset), Math.Clamp(limit <= 0 ? 20 : limit, 1, 100));
}
