using Microsoft.AspNetCore.Mvc;
using Oficina.Application.Requests.ItemCatalogo;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Controllers;
[ApiController, Route("api/itens-catalogo")]
public sealed class ItemCatalogoController(IItemCatalogoService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<ResultadoPaginadoDto<ItemCatalogoDto>>> Listar(string? term, int offset = 0, int limit = 20, CancellationToken ct = default) => Ok(await service.ListarAsync(term, offset, limit, ct));
    [HttpGet("{id:guid}")] public async Task<ActionResult<ItemCatalogoDto>> ObterPorId(Guid id, CancellationToken ct) { var r = await service.ObterPorIdAsync(id, ct); return r is null ? NotFound() : Ok(r); }
    [HttpPost] public async Task<ActionResult<Guid>> Criar(CriarItemCatalogoRequest request, CancellationToken ct) => Ok(await service.CriarAsync(request.Nome, request.Preco, request.Tipo, request.QuantidadeEstoque, ct));
    [HttpPut("{id:guid}")] public async Task<ActionResult<Guid>> Atualizar(Guid id, AtualizarItemCatalogoRequest request, CancellationToken ct) => Ok(await service.AtualizarAsync(id, request.Nome, request.Preco, request.Tipo, request.QuantidadeEstoque, ct));
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Excluir(Guid id, CancellationToken ct) => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound();
}
