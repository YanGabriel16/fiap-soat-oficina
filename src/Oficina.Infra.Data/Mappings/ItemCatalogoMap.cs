using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Mappings;

public sealed class ItemCatalogoMap : EntidadeBaseMap<ItemCatalogo>
{
    public override void Configure(EntityTypeBuilder<ItemCatalogo> b)
    {
        base.Configure(b);
        b.ToTable("itens_catalogo", "oficina");
        b.Property(x => x.Nome).HasMaxLength(160).IsRequired();
        b.Property(x => x.Preco).HasPrecision(18, 2).IsRequired();
        b.Property(x => x.Tipo).HasConversion<int>().IsRequired();
        b.Property(x => x.QuantidadeEstoque).IsRequired();
    }
}
