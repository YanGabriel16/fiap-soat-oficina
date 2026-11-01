using Microsoft.EntityFrameworkCore;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class OrdemServicoRepository(OficinaDbContext db) : IOrdemServicoRepository
{
    public Task<OrdemServico?> ObterPorIdAsync(Guid id, CancellationToken ct) => db.OrdensServico.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<(IReadOnlyList<OrdemServico> Itens, int Total)> ListarAsync(string? term, int offset, int limit, CancellationToken ct) { var q = db.OrdensServico.AsNoTracking().Where(x => string.IsNullOrEmpty(term) || x.Id.ToString().Contains(term!)); var total = await q.CountAsync(ct); return (await q.OrderByDescending(x => x.DataCriacao).Skip(offset).Take(limit).ToListAsync(ct), total); }
    public async Task AdicionarAsync(OrdemServico ordem, CancellationToken ct) { await db.OrdensServico.AddAsync(ordem, ct); await db.SaveChangesAsync(ct); }
    public async Task AtualizarAsync(OrdemServico ordem, CancellationToken ct) { db.OrdensServico.Update(ordem); await db.SaveChangesAsync(ct); }
    public async Task ExcluirAsync(OrdemServico ordem, CancellationToken ct) { db.OrdensServico.Update(ordem); await db.SaveChangesAsync(ct); }
}
