using Oficina.Domain.Common;

namespace Oficina.Domain.Entities;

public sealed class Usuario : EntidadeBase
{
    private Usuario() { }

    public Usuario(string nome, string email, string senhaHash)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
    }

    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;

    public void AtualizarSenha(string senhaHash)
    {
        SenhaHash = senhaHash;
        AtualizarDataModificacao();
    }
}
