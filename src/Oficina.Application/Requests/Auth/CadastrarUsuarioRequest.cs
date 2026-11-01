namespace Oficina.Application.Requests.Auth;
using Oficina.Domain.Enums;

/// <summary>Dados necessários para cadastrar um usuário.</summary>
public sealed record CadastrarUsuarioRequest
{
    [Required, MaxLength(160)]
    public required string Nome { get; init; }

    [Required, EmailAddress, MaxLength(320)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(128)]
    public required string Senha { get; init; }

    public RoleUsuario Role { get; init; } = RoleUsuario.Cliente;
    public string? Documento { get; init; }
}
