using Oficina.Domain.Enums;
namespace Oficina.Domain.Dtos;
public sealed record OrdemServicoDto(Guid Id, Guid ClienteId, Guid VeiculoId, StatusOrdemServico Status, decimal ValorTotal, DateTimeOffset DataCriacao, DateTimeOffset DataUltimaModificacao);
