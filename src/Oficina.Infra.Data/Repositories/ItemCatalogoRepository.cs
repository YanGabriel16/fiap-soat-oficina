using Microsoft.EntityFrameworkCore;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ItemCatalogoRepository(OficinaDbContext db) : IItemCatalogoRepository
{
    public Task<ItemCatalogo?> ObterPorIdAsync(Guid id, CancellationToken ct) => db.ItensCatalogo.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<(IReadOnlyList<ItemCatalogo> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { var q = db.ItensCatalogo.AsNoTracking().Where(x => string.IsNullOrEmpty(term) || x.Nome.ToLower().Contains(term!.ToLower())); var total = await q.CountAsync(ct); return (await q.OrderBy(x => x.Nome).Skip(offset).Take(limit).ToListAsync(ct), total); }
    public async Task AdicionarAsync(ItemCatalogo item, CancellationToken ct) { await db.ItensCatalogo.AddAsync(item, ct); await db.SaveChangesAsync(ct); }
    public async Task AtualizarAsync(ItemCatalogo item, CancellationToken ct) { db.ItensCatalogo.Update(item); await db.SaveChangesAsync(ct); }
    public async Task ExcluirAsync(ItemCatalogo item, CancellationToken ct) { db.ItensCatalogo.Update(item); await db.SaveChangesAsync(ct); }
}
