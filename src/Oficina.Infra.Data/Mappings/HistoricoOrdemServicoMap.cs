using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class HistoricoOrdemServicoMap : EntidadeBaseMap<HistoricoOrdemServico>
{
    public override void Configure(EntityTypeBuilder<HistoricoOrdemServico> b)
    {
        base.Configure(b); b.ToTable("historico_ordens_servico", "oficina"); b.Property(x => x.StatusAnterior).HasConversion<int?>(); b.Property(x => x.NovoStatus).HasConversion<int>(); b.Property(x => x.Tipo).HasConversion<int>(); b.Property(x => x.Observacao).HasMaxLength(2000);
        b.HasOne(x => x.OrdemServico).WithMany(x => x.Historico).HasForeignKey(x => x.OrdemServicoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Restrict);
    }
}
