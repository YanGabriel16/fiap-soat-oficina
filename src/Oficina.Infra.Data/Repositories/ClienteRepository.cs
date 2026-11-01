using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ClienteRepository(OficinaDbContext db) : IClienteRepository
{
    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct) => db.Clientes.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<(IReadOnlyList<Cliente> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { var q = db.Clientes.AsNoTracking().Where(x => string.IsNullOrEmpty(term) || x.Nome.ToLower().Contains(term!.ToLower()) || x.Documento.Contains(term!)); var total = await q.CountAsync(ct); return (await q.OrderBy(x => x.Nome).Skip(offset).Take(limit).ToListAsync(ct), total); }
    public async Task AdicionarAsync(Cliente cliente, CancellationToken ct) { await db.Clientes.AddAsync(cliente, ct); await db.SaveChangesAsync(ct); }
    public async Task AtualizarAsync(Cliente cliente, CancellationToken ct) { db.Clientes.Update(cliente); await db.SaveChangesAsync(ct); }
    public async Task ExcluirAsync(Cliente cliente, CancellationToken ct) { db.Clientes.Update(cliente); await db.SaveChangesAsync(ct); }
}
