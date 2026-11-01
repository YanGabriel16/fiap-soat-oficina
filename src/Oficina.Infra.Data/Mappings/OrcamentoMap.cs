using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class OrcamentoMap : EntidadeBaseMap<Orcamento>
{
    public override void Configure(EntityTypeBuilder<Orcamento> b)
    {
        base.Configure(b); b.ToTable("orcamentos", "oficina");
        b.Property(x => x.ValorMaoDeObra).HasPrecision(18, 2).IsRequired(); b.Property(x => x.Status).HasConversion<int>().IsRequired();
        b.HasIndex(x => x.OrdemServicoId).IsUnique();
        b.HasOne(x => x.OrdemServico).WithOne(x => x.Orcamento).HasForeignKey<Orcamento>(x => x.OrdemServicoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
    }
}
