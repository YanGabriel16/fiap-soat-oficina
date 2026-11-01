using Oficina.Domain.Enums;
namespace Oficina.Domain.Dtos;
public sealed record ItemCatalogoDto(Guid Id, string Nome, decimal Preco, TipoItemCatalogo Tipo, int QuantidadeEstoque, DateTimeOffset DataCriacao, DateTimeOffset DataUltimaModificacao);
