using Oficina.Domain.Entities;

namespace Oficina.Infra.Data.Mappings;

public sealed class TokenMap : EntidadeBaseMap<Token>
{
    public override void Configure(EntityTypeBuilder<Token> builder)
    {
        base.Configure(builder);
        builder.ToTable("token", "auth");
        builder.Property(x => x.Valor).HasMaxLength(4096).IsRequired();
        builder.Property(x => x.ExpiraEm).IsRequired();
        builder.Property(x => x.Revogado).IsRequired();
        builder.HasIndex(x => x.Valor).IsUnique();
        builder.HasIndex(x => x.UsuarioId);
        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
