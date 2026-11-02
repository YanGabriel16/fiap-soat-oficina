using Oficina.Domain.Enums;

namespace Oficina.Domain.Dtos;

/// <summary>Representa uma alteração registrada no histórico de um item do catálogo.</summary>
public sealed record HistoricoItemCatalogoDto(
    Guid Id,
    Guid ItemCatalogoId,
    AcaoHistoricoItemCatalogo Acao,
    string Nome,
    decimal Preco,
    TipoItemCatalogo Tipo,
    int QuantidadeEstoque,
    DateTimeOffset DataEvento);
