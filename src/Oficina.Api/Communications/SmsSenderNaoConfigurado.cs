using Oficina.Domain.Interfaces.Services;

namespace Oficina.Api.Communications;

public sealed class SmsSenderNaoConfigurado : ISmsSender
{
    public Task EnviarAsync(string destinatario, string mensagem, CancellationToken cancellationToken)
        => throw new InvalidOperationException("O provedor de SMS ainda não foi configurado.");
}
