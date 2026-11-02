using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class HistoricoItemCatalogoMap : IEntityTypeConfiguration<HistoricoItemCatalogo>
{
    public void Configure(EntityTypeBuilder<HistoricoItemCatalogo> builder)
    {
        builder.ToTable("historico_itens_catalogo", "auditoria");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Acao).HasConversion<int>().IsRequired();
        builder.Property(x => x.Nome).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Preco).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Tipo).HasConversion<int>().IsRequired();
        builder.Property(x => x.QuantidadeEstoque).IsRequired();
        builder.Property(x => x.DataEvento).IsRequired();
        builder.HasIndex(x => new { x.ItemCatalogoId, x.DataEvento });
    }
}
