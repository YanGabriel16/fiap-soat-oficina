using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public sealed class TokenService(IConfiguration configuration, ITokenRepository repository) : ITokenService
{
    public async Task<string> GerarAsync(Usuario usuario, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiracao = DateTimeOffset.UtcNow.AddHours(1);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiracao.UtcDateTime,
            signingCredentials: credentials);

        var valor = new JwtSecurityTokenHandler().WriteToken(token);
        await repository.AdicionarAsync(new Token(usuario.Id, valor, expiracao), ct);
        return valor;
    }
}
