using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class OrcamentoItemMap : EntidadeBaseMap<OrcamentoItem>
{
    public override void Configure(EntityTypeBuilder<OrcamentoItem> b)
    {
        base.Configure(b); b.ToTable("orcamentos_itens", "oficina"); b.Property(x => x.ValorUnitario).HasPrecision(18, 2).IsRequired();
        b.HasOne<Orcamento>().WithMany(x => x.Itens).HasForeignKey(x => x.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.ItemCatalogo).WithMany().HasForeignKey(x => x.ItemCatalogoId).OnDelete(DeleteBehavior.Restrict);
    }
}
