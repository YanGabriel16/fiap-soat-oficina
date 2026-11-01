using Oficina.Domain.Common;

namespace Oficina.Domain.Entities;
using Oficina.Domain.Enums;

public sealed class Usuario : EntidadeBase
{
    private Usuario() { }

    public Usuario(string nome, string email, string senhaHash, RoleUsuario role = RoleUsuario.Cliente)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
    }

    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public RoleUsuario Role { get; private set; }
    public Cliente? Cliente { get; private set; }

    public void AlterarRole(RoleUsuario role) { Role = role; AtualizarDataModificacao(); }

    public void AtualizarSenha(string senhaHash)
    {
        SenhaHash = senhaHash;
        AtualizarDataModificacao();
    }
}
