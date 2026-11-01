using Microsoft.AspNetCore.Identity;
using Oficina.Domain.Common;
using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public sealed class AuthService(
    IUsuarioRepository repository,
    IPasswordHasher<Usuario> passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<Guid> CadastrarAsync(string nome, string email, string senha, CancellationToken ct)
    {
        email = email.Trim().ToLowerInvariant();

        if (await repository.ExistePorEmailAsync(email, ct))
            throw new ConflitoException("Já existe um usuário cadastrado com este e-mail.");

        var usuario = new Usuario(nome.Trim(), email, string.Empty);
        usuario.AtualizarSenha(passwordHasher.HashPassword(usuario, senha));
        await repository.AdicionarAsync(usuario, ct);
        return usuario.Id;
    }

    public async Task<LoginDto?> LoginAsync(string email, string senha, CancellationToken ct)
    {
        var usuario = await repository.ObterPorEmailAsync(email.Trim().ToLowerInvariant(), ct);
        if (usuario is null)
            return null;

        var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
        if (resultado == PasswordVerificationResult.Failed)
            return null;

        return new LoginDto(tokenService.Gerar(usuario), DateTimeOffset.UtcNow.AddHours(1));
    }
}
