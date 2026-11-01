using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Mappings;

public sealed class OrdemServicoMap : EntidadeBaseMap<OrdemServico>
{
    public override void Configure(EntityTypeBuilder<OrdemServico> b)
    {
        base.Configure(b);
        b.ToTable("ordens_servico", "oficina");
        b.Property(x => x.Status).HasConversion<int>().IsRequired();
        b.Property(x => x.ValorTotal).HasPrecision(18, 2).IsRequired();
        b.Property(x => x.ValorMaoDeObra).HasPrecision(18, 2).IsRequired();
        b.Property(x => x.Descricao).HasMaxLength(2000).IsRequired();
        b.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Atendente).WithMany().HasForeignKey(x => x.AtendenteId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x => new { x.ClienteId, x.VeiculoId });
    }
}
