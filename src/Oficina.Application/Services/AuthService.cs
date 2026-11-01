using Microsoft.AspNetCore.Identity;
using Oficina.Domain.Common;
using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
using Oficina.Domain.Enums;

namespace Oficina.Application.Services;

public sealed class AuthService(
    IUsuarioRepository repository,
    IClienteRepository clienteRepository,
    IPasswordHasher<Usuario> passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<Guid> CadastrarAsync(string nome, string email, string senha, CancellationToken ct)
        => await CadastrarAsync(nome, email, senha, RoleUsuario.Cliente, null, ct);

    public async Task<Guid> CadastrarAsync(string nome, string email, string senha, RoleUsuario role, string? documento, CancellationToken ct)
    {
        email = email.Trim().ToLowerInvariant();

        if (await repository.ExistePorEmailAsync(email, ct))
            throw new ConflitoException("Já existe um usuário cadastrado com este e-mail.");

        if (role == RoleUsuario.Cliente && string.IsNullOrWhiteSpace(documento)) throw new ArgumentException("Documento é obrigatório para clientes.");
        if (documento is not null && !Validadores.Documento(documento)) throw new ArgumentException("CPF ou CNPJ inválido.");
        var usuario = new Usuario(nome.Trim(), email, string.Empty, role);
        usuario.AtualizarSenha(passwordHasher.HashPassword(usuario, senha));
        await repository.AdicionarAsync(usuario, ct);
        if (role == RoleUsuario.Cliente)
            await clienteRepository.AdicionarAsync(new Cliente(nome.Trim(), documento!, usuario.Id), ct);
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

        return new LoginDto(await tokenService.GerarAsync(usuario, ct), DateTimeOffset.UtcNow.AddHours(1));
    }
}
