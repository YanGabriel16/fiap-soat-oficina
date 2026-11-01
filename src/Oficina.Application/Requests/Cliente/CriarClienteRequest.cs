namespace Oficina.Application.Requests.Cliente;
public sealed record CriarClienteRequest([Required, MaxLength(160)] string Nome, [Required, MaxLength(18)] string Documento);
