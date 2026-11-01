using Oficina.Domain.Enums;

namespace Oficina.Domain.Dtos;

public sealed record HistoricoOrdemServicoDto(Guid Id, Guid OrdemServicoId, StatusOrdemServico? StatusAnterior, StatusOrdemServico NovoStatus, string? Observacao, Guid UsuarioId, string UsuarioNome, DateTimeOffset DataCriacao, TipoHistoricoOrdem Tipo);
