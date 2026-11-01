using Microsoft.AspNetCore.Authorization;
using Oficina.Application.Requests.Auth;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

[ApiController, Route("api/auth")]
public sealed class AuthController(IAuthService service) : ControllerBase
{
    [AllowAnonymous, HttpPost("usuarios")]
    public async Task<ActionResult<Guid>> Cadastrar([FromBody] CadastrarUsuarioRequest request, CancellationToken ct)
    {
        var id = await service.CadastrarAsync(request.Nome, request.Email, request.Senha, ct);
        return StatusCode(StatusCodes.Status201Created, id);
    }

    [AllowAnonymous, HttpPost("login")]
    public async Task<ActionResult<LoginDto>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var resultado = await service.LoginAsync(request.Email, request.Senha, ct);
        return resultado is null ? Unauthorized() : Ok(resultado);
    }
}
