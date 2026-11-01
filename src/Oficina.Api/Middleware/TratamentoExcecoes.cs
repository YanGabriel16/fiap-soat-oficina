using Oficina.Domain.Common;
using Oficina.Domain.Dtos;

namespace Oficina.Api.Middleware;

public sealed class TratamentoExcecoes(ILogger<TratamentoExcecoes> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, mensagem) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Dados inválidos."),
            ConflitoException => (StatusCodes.Status409Conflict, "Operação não permitida."),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.")
        };
        logger.LogError(exception, "Erro tratado pela API: {Mensagem}", mensagem);
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ErroResponse(status, mensagem, context.Request.Path), ct);
        return true;
    }
}
