using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;

namespace Oficina.Infra.Data.Repositories;

public sealed class RelatorioRepository(OficinaDbContext db) : IRelatorioRepository
{
    public async Task<IReadOnlyList<OrdemServico>> ListarOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct)
        => await (from ordem in db.OrdensServico
                        .AsNoTracking()
                        .Include(x => x.Historico)
                  where (!inicio.HasValue || ordem.DataCriacao >= inicio.Value)
                        && (!fim.HasValue || ordem.DataCriacao <= fim.Value)
                  orderby ordem.DataCriacao
                  select ordem).ToListAsync(ct);

    public async Task<IReadOnlyList<Orcamento>> ListarOrcamentosAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct)
        => await (from orcamento in db.Orcamentos.AsNoTracking()
                  where (!inicio.HasValue || orcamento.DataCriacao >= inicio.Value)
                        && (!fim.HasValue || orcamento.DataCriacao <= fim.Value)
                  orderby orcamento.DataCriacao
                  select orcamento).ToListAsync(ct);

    public async Task<IReadOnlyList<ItemCatalogo>> ListarEstoqueAsync(int limite, CancellationToken ct)
        => await (from item in db.ItensCatalogo.AsNoTracking()
                  orderby item.QuantidadeEstoque, item.Nome
                  select item).Take(limite).ToListAsync(ct);
}
