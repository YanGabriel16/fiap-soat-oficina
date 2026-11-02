using Oficina.Application.Requests.ItemCatalogo;
using Oficina.Domain.Dtos;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Controllers;

[ApiController, Route("api/itens-catalogo")]
[Authorize(Roles = "Admin,Atendente")]
public sealed class ItemCatalogoController(IItemCatalogoService service) : ApiControllerBase
{
    [HttpGet]
    public Task<IActionResult> Listar(
        [FromQuery] string? term,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 20,
        CancellationToken ct = default) 
        => ExecutarAsync(async () => Ok(await service.ListarAsync(term, offset, limit, ct)));

    [HttpGet("{id:guid}")]
    public Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken ct) 
        => ExecutarAsync(async () =>
        {
            var resultado = await service.ObterPorIdAsync(id, ct);
            return resultado is null ? NotFound() : Ok(resultado);
        });

    [HttpGet("{id:guid}/historico")]
    public Task<IActionResult> Historico([FromRoute] Guid id, CancellationToken ct)
        => ExecutarAsync(async () => Ok(await service.ListarHistoricoAsync(id, ct)));

    [HttpPost]
    public Task<IActionResult> Criar([FromBody] CriarItemCatalogoRequest request, CancellationToken ct) 
        => ExecutarAsync(async () => Ok(await service.CriarAsync(request.Nome, request.Preco, request.Tipo, request.QuantidadeEstoque, ct)));

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarItemCatalogoRequest request,
        CancellationToken ct) 
        => ExecutarAsync(async () => Ok(await service.AtualizarAsync(id, request.Nome, request.Preco, request.Tipo, request.QuantidadeEstoque, ct)));

    [HttpPost("{id:guid}/estoque")]
    public Task<IActionResult> AdicionarEstoque(
        [FromRoute] Guid id,
        [FromBody] AdicionarEstoqueRequest request,
        CancellationToken ct)
        => ExecutarAsync(async () => Ok(await service.AdicionarEstoqueAsync(id, request.Quantidade, ct)));

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken ct) 
        => ExecutarAsync(async () => await service.ExcluirAsync(id, ct) ? NoContent() : NotFound());
}
