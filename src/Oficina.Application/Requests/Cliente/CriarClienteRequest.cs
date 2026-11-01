namespace Oficina.Application.Requests.Cliente;
using Oficina.Domain.Enums;
public sealed record CriarClienteRequest([Required, MaxLength(160)] string Nome, [Required, MaxLength(18)] string Documento)
{
    public Guid? UsuarioId { get; init; }
}
