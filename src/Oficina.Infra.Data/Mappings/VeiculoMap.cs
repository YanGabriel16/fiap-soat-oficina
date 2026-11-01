using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Mappings;
public sealed class VeiculoMap : EntidadeBaseMap<Veiculo> { public override void Configure(EntityTypeBuilder<Veiculo> b) { base.Configure(b); b.ToTable("veiculos"); b.Property(x => x.Placa).HasMaxLength(8).IsRequired(); b.Property(x => x.Marca).HasMaxLength(80).IsRequired(); b.Property(x => x.Modelo).HasMaxLength(80).IsRequired(); b.HasIndex(x => x.Placa).IsUnique(); b.HasOne(x => x.Cliente).WithMany(x => x.Veiculos).HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict); } }
