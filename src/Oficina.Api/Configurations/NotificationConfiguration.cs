using Oficina.Api.Communications;
using Oficina.Domain.Interfaces.Services;
using MimeKit;

namespace Oficina.Api.Configurations;

public static class NotificationConfiguration
{
    public static IServiceCollection AddNotificationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection("Email"))
            .Validate(options => !options.Enabled ||
                (!string.IsNullOrWhiteSpace(options.Host)
                && options.Port is > 0 and <= 65535
                && MailboxAddress.TryParse(options.FromAddress, out _)),
                "Configure Email:Host, Email:Port e Email:FromAddress para habilitar o envio.")
            .Validate(options => !options.Enabled ||
                (string.IsNullOrWhiteSpace(options.Username) == string.IsNullOrWhiteSpace(options.Password)),
                "Email:Username e Email:Password devem ser configurados juntos.")
            .ValidateOnStart();

        services.Configure<SmsOptions>(configuration.GetSection("Sms"));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ISmsSender, SmsSenderNaoConfigurado>();
        services.AddScoped<INotificationQueue, HangfireNotificationQueue>();

        return services;
    }
}
