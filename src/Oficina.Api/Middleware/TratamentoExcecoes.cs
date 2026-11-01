using Oficina.Domain.Common;

namespace Oficina.Api.Middleware;

public sealed class TratamentoExcecoes(ILogger<TratamentoExcecoes> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, titulo) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Dados inválidos."),
            ConflitoException => (StatusCodes.Status409Conflict, "Operação não permitida."),
            _ => (0, string.Empty)
        };
        if (status == 0) return false;
        logger.LogWarning(exception, "Exceção de negócio tratada: {Titulo}", titulo);
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = titulo, Detail = titulo, Instance = context.Request.Path }, ct);
        return true;
    }
}
