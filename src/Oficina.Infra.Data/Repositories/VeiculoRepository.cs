using Microsoft.EntityFrameworkCore;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class VeiculoRepository(OficinaDbContext db) : IVeiculoRepository
{
    public Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken ct) => db.Veiculos.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<(IReadOnlyList<Veiculo> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { var q = db.Veiculos.AsNoTracking().Where(x => string.IsNullOrEmpty(term) || x.Placa.Contains(term!) || x.Marca.ToLower().Contains(term!.ToLower()) || x.Modelo.ToLower().Contains(term!.ToLower())); var total = await q.CountAsync(ct); return (await q.OrderBy(x => x.Placa).Skip(offset).Take(limit).ToListAsync(ct), total); }
    public async Task AdicionarAsync(Veiculo veiculo, CancellationToken ct) { await db.Veiculos.AddAsync(veiculo, ct); await db.SaveChangesAsync(ct); }
    public async Task AtualizarAsync(Veiculo veiculo, CancellationToken ct) { db.Veiculos.Update(veiculo); await db.SaveChangesAsync(ct); }
    public async Task ExcluirAsync(Veiculo veiculo, CancellationToken ct) { db.Veiculos.Update(veiculo); await db.SaveChangesAsync(ct); }
}
