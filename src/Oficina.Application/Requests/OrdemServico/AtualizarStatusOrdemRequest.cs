using Oficina.Domain.Enums;

namespace Oficina.Application.Requests.OrdemServico;

public sealed record AtualizarStatusOrdemRequest(StatusOrdemServico Status, string? Observacao);
