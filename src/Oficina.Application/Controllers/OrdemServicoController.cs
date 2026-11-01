using Oficina.Application.Requests.OrdemServico;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Oficina.Application.Controllers;

[ApiController, Route("api/ordens-servico")]
public sealed class OrdemServicoController(IOrdemServicoService service, IOrcamentoService? orcamentoService = null, IHistoricoOrdemServicoService? historicoService = null) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> Listar(
        [FromQuery] string? term,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 20,
        CancellationToken ct = default) =>
        ExecutarAsync(async () => Ok(await service.ListarAsync(term, offset, limit, ct)));

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken ct) =>
        ExecutarAsync(async () =>
        {
            var resultado = await service.ObterPorIdAsync(id, ct);
            return resultado is null ? NotFound() : Ok(resultado);
        });

    [HttpPost]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> Criar([FromBody] CriarOrdemServicoRequest request, CancellationToken ct) =>
        ExecutarAsync(async () =>
        {
            if (User?.Identity?.IsAuthenticated != true) return Ok(await service.CriarAsync(request.ClienteId, request.VeiculoId, ct));
            var atendenteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Sid) ?? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
            var id = await service.CriarComOrcamentoAsync(request.ClienteId, request.VeiculoId, atendenteId, request.Descricao, request.Itens.Select(x => (x.ItemCatalogoId, x.Quantidade)).ToList(), request.ValorMaoDeObra, ct);
            return StatusCode(StatusCodes.Status201Created, id);
        });

    [HttpGet("minhas")]
    [Authorize(Roles = "Cliente")]
    public Task<IActionResult> Minhas([FromQuery] int offset = 0, [FromQuery] int limit = 20, CancellationToken ct = default) => ExecutarAsync(async () => Ok(await service.ListarDoClienteAsync(UsuarioId(), offset, limit, ct)));

    [HttpGet("por-cliente")]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> PorCliente([FromQuery, Required] string nome, [FromQuery] int offset = 0, [FromQuery] int limit = 20, CancellationToken ct = default) => ExecutarAsync(async () => Ok(await service.ListarPorNomeClienteAsync(nome, offset, limit, ct)));

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> AtualizarStatus(Guid id, AtualizarStatusOrdemRequest request, CancellationToken ct) => ExecutarAsync(async () => { await service.AtualizarStatusAsync(id, request.Status, UsuarioId(), request.Observacao, ct); return NoContent(); });

    [HttpGet("{id:guid}/historico")]
    [Authorize(Roles = "Admin,Atendente,Cliente")]
    public Task<IActionResult> Historico(Guid id, CancellationToken ct) => ExecutarAsync(async () => Ok(await (historicoService ?? throw new InvalidOperationException()).ListarAsync(id, UsuarioId(), User.IsInRole("Cliente"), ct)));

    [HttpGet("{id:guid}/orcamento")]
    [Authorize(Roles = "Admin,Atendente,Cliente")]
    public Task<IActionResult> Orcamento(Guid id, CancellationToken ct) => ExecutarAsync(async () => { var resultado = await (orcamentoService ?? throw new InvalidOperationException()).ObterPorOrdemAsync(id, UsuarioId(), User.IsInRole("Cliente"), ct); return resultado is null ? NotFound() : Ok(resultado); });

    [HttpPost("{id:guid}/orcamento/decisao")]
    [Authorize(Roles = "Cliente")]
    public Task<IActionResult> DecidirOrcamento(Guid id, DecidirOrcamentoRequest request, CancellationToken ct) => ExecutarAsync(async () => { await (orcamentoService ?? throw new InvalidOperationException()).DecidirAsync(id, UsuarioId(), request.Decisao, ct); return NoContent(); });

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarOrdemServicoRequest request,
        CancellationToken ct) =>
        ExecutarAsync(async () => Ok(await service.AtualizarAsync(id, request.ClienteId, request.VeiculoId, request.Status, request.ValorTotal, ct)));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Atendente")]
    public Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken ct) =>
        ExecutarAsync(async () => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound());

    private Guid UsuarioId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? throw new UnauthorizedAccessException());
}
