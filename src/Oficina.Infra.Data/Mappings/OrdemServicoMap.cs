using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Mappings;
public sealed class OrdemServicoMap : EntidadeBaseMap<OrdemServico> { public override void Configure(EntityTypeBuilder<OrdemServico> b) { base.Configure(b); b.ToTable("ordens_servico"); b.Property(x => x.Status).HasConversion<int>().IsRequired(); b.Property(x => x.ValorTotal).HasPrecision(18, 2).IsRequired(); b.HasIndex(x => new { x.ClienteId, x.VeiculoId }); } }
