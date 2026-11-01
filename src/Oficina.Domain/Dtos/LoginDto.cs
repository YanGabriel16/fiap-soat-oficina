namespace Oficina.Domain.Dtos;

public sealed record LoginDto(string Token, DateTimeOffset ExpiraEm);
