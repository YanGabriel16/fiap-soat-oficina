using System.ComponentModel.DataAnnotations;
namespace Oficina.Application.Requests.Veiculo;
public sealed record CriarVeiculoRequest([Required] Guid ClienteId, [Required, MaxLength(8)] string Placa, [Required, MaxLength(80)] string Marca, [Required, MaxLength(80)] string Modelo, [Range(1900, 2100)] int Ano);
