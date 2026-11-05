using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Api.Communications;

public sealed class SmtpEmailSender(IOptions<EmailOptions> options) : IEmailSender
{
    public async Task EnviarAsync(string destinatario, string assunto, string mensagem, CancellationToken cancellationToken)
    {
        var settings = options.Value;
        using var email = new MimeMessage();
        email.From.Add(new MailboxAddress(settings.FromName, settings.FromAddress));
        email.To.Add(MailboxAddress.Parse(destinatario));
        email.Subject = assunto;
        email.Body = new TextPart("plain") { Text = mensagem };

        using var client = new SmtpClient();
        var security = settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;
        await client.ConnectAsync(settings.Host, settings.Port, security, cancellationToken);

        if (settings.Username is { Length: > 0 } username && settings.Password is { Length: > 0 } password)
            await client.AuthenticateAsync(username, password, cancellationToken);

        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
