namespace Oficina.Application.Requests.Cliente;
using Oficina.Domain.Enums;
public sealed record CriarClienteRequest(
    [Required, MaxLength(160)] string Nome,
    [Required, MaxLength(18)] string Documento,
    [Phone, MaxLength(24)] string? Telefone = null)
{
    public Guid? UsuarioId { get; init; }
}
