namespace Oficina.Application.Requests.Auth;

/// <summary>Dados necessários para cadastrar um usuário.</summary>
public sealed record CadastrarUsuarioRequest
{
    [Required, MaxLength(160)]
    public required string Nome { get; init; }

    [Required, EmailAddress, MaxLength(320)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(128)]
    public required string Senha { get; init; }
}
