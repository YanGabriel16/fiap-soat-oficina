namespace Oficina.Domain.Dtos;
public sealed record ResultadoPaginadoDto<T>(IReadOnlyList<T> Itens, int Offset, int Limit, int Total);
