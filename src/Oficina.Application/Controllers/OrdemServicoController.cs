using Microsoft.AspNetCore.Mvc;
using Oficina.Application.Requests.OrdemServico;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Controllers;
[ApiController, Route("api/ordens-servico")]
public sealed class OrdemServicoController(IOrdemServicoService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<ResultadoPaginadoDto<OrdemServicoDto>>> Listar(string? term, int offset = 0, int limit = 20, CancellationToken ct = default) => Ok(await service.ListarAsync(term, offset, limit, ct));
    [HttpGet("{id:guid}")] public async Task<ActionResult<OrdemServicoDto>> ObterPorId(Guid id, CancellationToken ct) { var r = await service.ObterPorIdAsync(id, ct); return r is null ? NotFound() : Ok(r); }
    [HttpPost] public async Task<ActionResult<Guid>> Criar(CriarOrdemServicoRequest request, CancellationToken ct) => Ok(await service.CriarAsync(request.ClienteId, request.VeiculoId, ct));
    [HttpPut("{id:guid}")] public async Task<ActionResult<Guid>> Atualizar(Guid id, AtualizarOrdemServicoRequest request, CancellationToken ct) => Ok(await service.AtualizarAsync(id, request.ClienteId, request.VeiculoId, request.Status, request.ValorTotal, ct));
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Excluir(Guid id, CancellationToken ct) => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound();
}
