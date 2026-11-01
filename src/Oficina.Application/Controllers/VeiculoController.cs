using Microsoft.AspNetCore.Mvc;
using Oficina.Application.Requests.Veiculo;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Controllers;
[ApiController, Route("api/veiculos")]
public sealed class VeiculoController(IVeiculoService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<ResultadoPaginadoDto<VeiculoDto>>> Listar(string? term, int offset = 0, int limit = 20, CancellationToken ct = default) => Ok(await service.ListarAsync(term, offset, limit, ct));
    [HttpGet("{id:guid}")] public async Task<ActionResult<VeiculoDto>> ObterPorId(Guid id, CancellationToken ct) { var r = await service.ObterPorIdAsync(id, ct); return r is null ? NotFound() : Ok(r); }
    [HttpPost] public async Task<ActionResult<Guid>> Criar(CriarVeiculoRequest request, CancellationToken ct) => Ok(await service.CriarAsync(request.ClienteId, request.Placa, request.Marca, request.Modelo, request.Ano, ct));
    [HttpPut("{id:guid}")] public async Task<ActionResult<Guid>> Atualizar(Guid id, AtualizarVeiculoRequest request, CancellationToken ct) => Ok(await service.AtualizarAsync(id, request.ClienteId, request.Placa, request.Marca, request.Modelo, request.Ano, ct));
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Excluir(Guid id, CancellationToken ct) => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound();
}
