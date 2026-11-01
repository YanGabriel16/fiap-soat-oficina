using Oficina.Domain.Common;

namespace Oficina.Domain.Entities;

public sealed class Token : EntidadeBase
{
    private Token() { }

    public Token(Guid usuarioId, string valor, DateTimeOffset expiraEm)
    {
        UsuarioId = usuarioId;
        Valor = valor;
        ExpiraEm = expiraEm;
    }

    public Guid UsuarioId { get; private set; }
    public string Valor { get; private set; } = null!;
    public DateTimeOffset ExpiraEm { get; private set; }
    public bool Revogado { get; private set; }

    public void Revogar()
    {
        Revogado = true;
        AtualizarDataModificacao();
    }
}
