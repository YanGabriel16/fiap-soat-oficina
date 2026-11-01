using System.ComponentModel.DataAnnotations;
using Oficina.Domain.Enums;
namespace Oficina.Application.Requests.OrdemServico;
public sealed record AtualizarOrdemServicoRequest([Required] Guid ClienteId, [Required] Guid VeiculoId, StatusOrdemServico Status, decimal ValorTotal);
