using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Mappings;
public sealed class ClienteMap : EntidadeBaseMap<Cliente> { public override void Configure(EntityTypeBuilder<Cliente> b) { base.Configure(b); b.ToTable("clientes"); b.Property(x => x.Nome).HasMaxLength(160).IsRequired(); b.Property(x => x.Documento).HasMaxLength(18).IsRequired(); b.HasIndex(x => x.Documento).IsUnique(); } }
