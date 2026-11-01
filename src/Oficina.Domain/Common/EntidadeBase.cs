namespace Oficina.Domain.Common;
public abstract class EntidadeBase
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset DataCriacao { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DataUltimaModificacao { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DataExclusao { get; protected set; }
    public bool Excluida => DataExclusao.HasValue;
    public void AtualizarDataModificacao() => DataUltimaModificacao = DateTimeOffset.UtcNow;
    public void Excluir() { DataExclusao = DateTimeOffset.UtcNow; AtualizarDataModificacao(); }
}
