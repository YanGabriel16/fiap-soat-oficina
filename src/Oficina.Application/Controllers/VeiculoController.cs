using Oficina.Application.Requests.Veiculo;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

[ApiController, Route("api/veiculos")]
public sealed class VeiculoController(IVeiculoService service) : ApiControllerBase
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
    public Task<IActionResult> Criar([FromBody] CriarVeiculoRequest request, CancellationToken ct) =>
        ExecutarAsync(async () => Ok(await service.CriarAsync(request.ClienteId, request.Placa, request.Marca, request.Modelo, request.Ano, ct)));

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarVeiculoRequest request,
        CancellationToken ct) =>
        ExecutarAsync(async () => Ok(await service.AtualizarAsync(id, request.ClienteId, request.Placa, request.Marca, request.Modelo, request.Ano, ct)));

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken ct) =>
        ExecutarAsync(async () => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound());
}
