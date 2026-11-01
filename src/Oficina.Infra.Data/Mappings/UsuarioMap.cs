using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class UsuarioMap : EntidadeBaseMap<Usuario>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);
        builder.ToTable("usuarios", "auth");
        builder.Property(x => x.Nome).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.Property(x => x.SenhaHash).HasMaxLength(512).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
