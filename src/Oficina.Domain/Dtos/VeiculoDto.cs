namespace Oficina.Domain.Dtos;
public sealed record VeiculoDto(Guid Id, Guid ClienteId, string Placa, string Marca, string Modelo, int Ano, DateTimeOffset DataCriacao, DateTimeOffset DataUltimaModificacao);
