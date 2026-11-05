namespace Oficina.Domain.Interfaces.Services;

public interface IEmailSender
{
    Task EnviarAsync(string destinatario, string assunto, string mensagem, CancellationToken cancellationToken);
}
