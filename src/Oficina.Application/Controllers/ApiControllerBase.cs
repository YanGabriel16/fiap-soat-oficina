using Microsoft.AspNetCore.Authorization;
using Oficina.Domain.Common;
using Oficina.Domain.Dtos;

namespace Oficina.Application.Controllers;

[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected async Task<IActionResult> ExecutarAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception exception)
        {
            var (status, mensagem) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado."),
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                ConflitoException => (StatusCodes.Status409Conflict, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.")
            };

            return StatusCode(status, new ErroResponse(status, mensagem, Request.Path));
        }
    }
}
