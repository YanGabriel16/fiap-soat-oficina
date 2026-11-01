using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public sealed class OrcamentoRepository(OficinaDbContext db) : IOrcamentoRepository
{
    public Task<Orcamento?> ObterDetalhadoAsync(Guid id, CancellationToken ct) => db.Orcamentos.Include(x => x.Itens).ThenInclude(x => x.ItemCatalogo).SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<Orcamento?> ObterPorOrdemAsync(Guid ordemServicoId, CancellationToken ct) => db.Orcamentos.Include(x => x.Itens).ThenInclude(x => x.ItemCatalogo).SingleOrDefaultAsync(x => x.OrdemServicoId == ordemServicoId, ct);
    public async Task AdicionarAsync(Orcamento orcamento, CancellationToken ct) { await db.Orcamentos.AddAsync(orcamento, ct); await db.SaveChangesAsync(ct); }
    public async Task AtualizarAsync(Orcamento orcamento, CancellationToken ct) { db.Orcamentos.Update(orcamento); await db.SaveChangesAsync(ct); }
}
