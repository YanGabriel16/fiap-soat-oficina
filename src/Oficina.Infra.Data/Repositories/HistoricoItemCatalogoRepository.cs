using Microsoft.EntityFrameworkCore;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public sealed class HistoricoItemCatalogoRepository(OficinaDbContext db) : IHistoricoItemCatalogoRepository
{
    public async Task AdicionarAsync(HistoricoItemCatalogo historico, CancellationToken ct)
    {
        await db.HistoricosItensCatalogo.AddAsync(historico, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<HistoricoItemCatalogo>> ListarPorItemAsync(Guid itemCatalogoId, CancellationToken ct)
        => await (from historico in db.HistoricosItensCatalogo.AsNoTracking()
                  where historico.ItemCatalogoId == itemCatalogoId
                  orderby historico.DataEvento descending
                  select historico)
            .ToListAsync(ct);
}
