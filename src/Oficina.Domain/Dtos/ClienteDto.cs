namespace Oficina.Domain.Dtos;
public sealed record ClienteDto(Guid Id, string Nome, string Documento, DateTimeOffset DataCriacao, DateTimeOffset DataUltimaModificacao);
