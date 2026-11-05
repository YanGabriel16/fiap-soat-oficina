namespace Oficina.Domain.Interfaces.Services;

public interface INotificationQueue
{
    void EnfileirarEmail(string destinatario, string assunto, string mensagem);
    void EnfileirarSms(string destinatario, string mensagem);
}
