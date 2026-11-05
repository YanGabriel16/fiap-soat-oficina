using Hangfire;
using Microsoft.Extensions.Options;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Api.Communications;

public sealed class HangfireNotificationQueue(
    IBackgroundJobClient backgroundJobs,
    IOptions<EmailOptions> emailOptions,
    IOptions<SmsOptions> smsOptions,
    ILogger<HangfireNotificationQueue> logger) : INotificationQueue
{
    public void EnfileirarEmail(string destinatario, string assunto, string mensagem)
    {
        if (!emailOptions.Value.Enabled)
        {
            logger.LogInformation("Envio de e-mail desativado; a notificação não foi enfileirada.");
            return;
        }

        backgroundJobs.Enqueue<IEmailSender>(sender =>
            sender.EnviarAsync(destinatario, assunto, mensagem, CancellationToken.None));
    }

    public void EnfileirarSms(string destinatario, string mensagem)
    {
        if (!smsOptions.Value.Enabled)
        {
            logger.LogInformation("Envio de SMS desativado; a notificação não foi enfileirada.");
            return;
        }

        backgroundJobs.Enqueue<ISmsSender>(sender =>
            sender.EnviarAsync(destinatario, mensagem, CancellationToken.None));
    }
}
