using Oficina.Domain.Common;
namespace Oficina.Infra.Data.Mappings;

public abstract class EntidadeBaseMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntidadeBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.DataUltimaModificacao).IsRequired();
        builder.Property(x => x.DataExclusao);
        builder.HasQueryFilter(x => x.DataExclusao == null);
    }
}
