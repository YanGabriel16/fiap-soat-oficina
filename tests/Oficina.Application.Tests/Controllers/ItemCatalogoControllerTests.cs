namespace Oficina.Application.Tests.Controllers;

public sealed class ItemCatalogoControllerTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static ItemCatalogoDto Item => new(Id, "Troca de óleo", 100m, TipoItemCatalogo.Servico, 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

    [Test]
    public async Task Listar_DeveRetornarOk()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.ListarAsync(null, 0, 20, It.IsAny<CancellationToken>())).ReturnsAsync(new ResultadoPaginadoDto<ItemCatalogoDto>([Item], 0, 20, 1));
        Assert.That(await new ItemCatalogoController(service.Object).Listar(null, 0, 20, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_DeveRetornarOk()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(Item);
        Assert.That(await new ItemCatalogoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync((ItemCatalogoDto?)null);
        Assert.That(await new ItemCatalogoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Criar_DeveRetornarOk()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.CriarAsync("Troca de óleo", 100m, TipoItemCatalogo.Servico, 1, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var request = new CriarItemCatalogoRequest("Troca de óleo", 100m, TipoItemCatalogo.Servico, 1);
        Assert.That(await new ItemCatalogoController(service.Object).Criar(request, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Atualizar_DeveRetornarOk()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.AtualizarAsync(Id, "Troca de óleo", 100m, TipoItemCatalogo.Servico, 1, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var request = new AtualizarItemCatalogoRequest("Troca de óleo", 100m, TipoItemCatalogo.Servico, 1);
        Assert.That(await new ItemCatalogoController(service.Object).Atualizar(Id, request, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Excluir_QuandoEncontrado_DeveRetornarNoContent()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Assert.That(await new ItemCatalogoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Excluir_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IItemCatalogoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Assert.That(await new ItemCatalogoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }
}
