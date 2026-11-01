using Oficina.Application.Requests.OrdemServico;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

[ApiController, Route("api/ordens-servico")]
public sealed class OrdemServicoController(IOrdemServicoService service) : ApiControllerBase
{
    [HttpGet]
    public Task<IActionResult> Listar(
        [FromQuery] string? term,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 20,
        CancellationToken ct = default) =>
        ExecutarAsync(async () => Ok(await service.ListarAsync(term, offset, limit, ct)));

    [HttpGet("{id:guid}")]
    public Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken ct) =>
        ExecutarAsync(async () =>
        {
            var resultado = await service.ObterPorIdAsync(id, ct);
            return resultado is null ? NotFound() : Ok(resultado);
        });

    [HttpPost]
    public Task<IActionResult> Criar([FromBody] CriarOrdemServicoRequest request, CancellationToken ct) =>
        ExecutarAsync(async () => Ok(await service.CriarAsync(request.ClienteId, request.VeiculoId, ct)));

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarOrdemServicoRequest request,
        CancellationToken ct) =>
        ExecutarAsync(async () => Ok(await service.AtualizarAsync(id, request.ClienteId, request.VeiculoId, request.Status, request.ValorTotal, ct)));

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken ct) =>
        ExecutarAsync(async () => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound());
}
