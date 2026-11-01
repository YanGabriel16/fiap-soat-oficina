namespace Oficina.Application.Requests.Auth;

/// <summary>Credenciais usadas para autenticar um usuário.</summary>
public sealed record LoginRequest
{
    [Required, EmailAddress, MaxLength(320)]
    public required string Email { get; init; }

    [Required, MaxLength(128)]
    public required string Senha { get; init; }
}
