using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

/// <summary>Endpoints de indicadores operacionais da oficina.</summary>
[ApiController, Route("api/relatorios")]
[Authorize(Roles = "Admin,Atendente")]
public sealed class RelatorioController(IRelatorioService service) : ApiControllerBase
{
    /// <summary>Retorna o tempo de ciclo das ordens de serviço.</summary>
    [HttpGet("ordens-servico/tempo")]
    public Task<IActionResult> TempoOrdens(
        [FromQuery] DateTimeOffset? inicio,
        [FromQuery] DateTimeOffset? fim,
        CancellationToken ct)
        => ExecutarAsync(async () => Ok(await service.ObterTempoOrdensAsync(inicio, fim, ct)));

    /// <summary>Retorna os tempos e o resumo de decisões dos orçamentos.</summary>
    [HttpGet("orcamentos")]
    public Task<IActionResult> Orcamentos(
        [FromQuery] DateTimeOffset? inicio,
        [FromQuery] DateTimeOffset? fim,
        CancellationToken ct)
        => ExecutarAsync(async () => Ok(await service.ObterOrcamentosAsync(inicio, fim, ct)));

    /// <summary>Retorna a quantidade de ordens agrupada por status.</summary>
    [HttpGet("ordens-servico/status")]
    public Task<IActionResult> StatusOrdens(
        [FromQuery] DateTimeOffset? inicio,
        [FromQuery] DateTimeOffset? fim,
        CancellationToken ct)
        => ExecutarAsync(async () => Ok(await service.ObterStatusOrdensAsync(inicio, fim, ct)));

    /// <summary>Retorna os itens com menor quantidade disponível no estoque.</summary>
    [HttpGet("estoque")]
    public Task<IActionResult> Estoque(
        [FromQuery] int limite = 10,
        CancellationToken ct = default)
        => ExecutarAsync(async () => Ok(await service.ObterEstoqueAsync(limite, ct)));
}
