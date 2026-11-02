using Oficina.Domain.Enums;

namespace Oficina.Domain.Dtos;

/// <summary>Resumo do tempo de ciclo de uma ordem de serviço.</summary>
public sealed record RelatorioTempoOrdemDto(
    Guid OrdemServicoId,
    StatusOrdemServico Status,
    DateTimeOffset Inicio,
    DateTimeOffset? Fim,
    decimal? DuracaoHoras);

/// <summary>Relatório consolidado dos tempos das ordens de serviço.</summary>
public sealed record RelatorioTempoOrdensResponse(
    int Total,
    int Finalizadas,
    decimal? MediaDuracaoHoras,
    IReadOnlyList<RelatorioTempoOrdemDto> Ordens);

/// <summary>Dados de duração e decisão de um orçamento.</summary>
public sealed record RelatorioOrcamentoDto(
    Guid OrcamentoId,
    Guid OrdemServicoId,
    StatusOrcamento Status,
    DateTimeOffset Inicio,
    DateTimeOffset? Fim,
    decimal? DuracaoHoras);

/// <summary>Relatório consolidado dos orçamentos.</summary>
public sealed record RelatorioOrcamentosResponse(
    int Total,
    int Aprovados,
    int Recusados,
    int Pendentes,
    decimal? MediaDuracaoDecisaoHoras,
    IReadOnlyList<RelatorioOrcamentoDto> Orcamentos);

/// <summary>Quantidade de ordens agrupadas por status.</summary>
public sealed record RelatorioStatusOrdemDto(StatusOrdemServico Status, int Quantidade);

/// <summary>Posição atual de um item de catálogo no estoque.</summary>
public sealed record RelatorioEstoqueDto(
    Guid ItemCatalogoId,
    string Nome,
    TipoItemCatalogo Tipo,
    int QuantidadeEstoque,
    decimal Preco);
