namespace Oficina.Domain.Dtos;

public sealed record ErroResponse(
    int Status,
    string Mensagem,
    string Instancia,
    IReadOnlyDictionary<string, string[]>? Erros = null);
