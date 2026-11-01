using Microsoft.AspNetCore.Authorization;
using Oficina.Application.Requests.Auth;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

[AllowAnonymous]
[ApiController, Route("api/auth")]
public sealed class AuthController(IAuthService service) : ApiControllerBase
{
    [HttpPost("usuarios")]
    public Task<IActionResult> Cadastrar([FromBody] CadastrarUsuarioRequest request, CancellationToken ct) =>
        ExecutarAsync(async () =>
        {
            var id = await service.CadastrarAsync(request.Nome, request.Email, request.Senha, ct);
            return StatusCode(StatusCodes.Status201Created, id);
        });

    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct) =>
        ExecutarAsync(async () =>
        {
            var resultado = await service.LoginAsync(request.Email, request.Senha, ct);
            return resultado is null ? Unauthorized() : Ok(resultado);
        });
}
