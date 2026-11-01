using System.ComponentModel.DataAnnotations;
namespace Oficina.Application.Requests.OrdemServico;
public sealed record CriarOrdemServicoRequest([Required] Guid ClienteId, [Required] Guid VeiculoId);
