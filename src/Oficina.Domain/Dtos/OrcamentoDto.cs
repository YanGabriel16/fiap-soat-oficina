using Oficina.Domain.Enums;

namespace Oficina.Domain.Dtos;

public sealed record OrcamentoDto(Guid Id, Guid OrdemServicoId, Guid ClienteId, decimal ValorMaoDeObra, decimal ValorTotal, StatusOrcamento Status, DateTimeOffset DataCriacao, DateTimeOffset? DataDecisao, IReadOnlyList<OrcamentoItemDto> Itens);
public sealed record OrcamentoItemDto(Guid ItemCatalogoId, string Nome, int Quantidade, decimal ValorUnitario, decimal ValorTotal);
