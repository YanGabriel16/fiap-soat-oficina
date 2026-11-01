using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class OrdemServicoItemMap : EntidadeBaseMap<OrdemServicoItem>
{
    public override void Configure(EntityTypeBuilder<OrdemServicoItem> b)
    {
        base.Configure(b); b.ToTable("ordens_servico_itens", "oficina");
        b.Property(x => x.ValorUnitario).HasPrecision(18, 2).IsRequired();
        b.HasOne<OrdemServico>().WithMany(x => x.Itens).HasForeignKey(x => x.OrdemServicoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.ItemCatalogo).WithMany().HasForeignKey(x => x.ItemCatalogoId).OnDelete(DeleteBehavior.Restrict);
    }
}
