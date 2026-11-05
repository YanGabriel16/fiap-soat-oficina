namespace Oficina.Domain.Interfaces.Services;

public interface ISmsSender
{
    Task EnviarAsync(string destinatario, string mensagem, CancellationToken cancellationToken);
}
