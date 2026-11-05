namespace Oficina.Application.Requests.Cliente;
public sealed record AtualizarClienteRequest(
    [Required, MaxLength(160)] string Nome,
    [Required, MaxLength(18)] string Documento,
    [Phone, MaxLength(24)] string? Telefone = null);
