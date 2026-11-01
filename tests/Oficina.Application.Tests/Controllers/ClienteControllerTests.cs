namespace Oficina.Application.Tests.Controllers;

public sealed class ClienteControllerTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static ClienteDto Cliente => new(Id, "Ana", "12345678901", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

    [Test]
    public async Task Listar_DeveRetornarOk() =>
        Assert.That(await ExecutarListar(), Is.TypeOf<OkObjectResult>());

    [Test]
    public async Task ObterPorId_DeveRetornarOk()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(Cliente);
        var resultado = await new ClienteController(service.Object).ObterPorId(Id, CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync((ClienteDto?)null);
        var resultado = await new ClienteController(service.Object).ObterPorId(Id, CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Criar_DeveRetornarOk()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.CriarAsync("Ana", "12345678901", It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var resultado = await new ClienteController(service.Object).Criar(new CriarClienteRequest("Ana", "12345678901"), CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Atualizar_DeveRetornarOk()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.AtualizarAsync(Id, "Ana", "12345678901", It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var resultado = await new ClienteController(service.Object).Atualizar(Id, new AtualizarClienteRequest("Ana", "12345678901"), CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Excluir_QuandoEncontrado_DeveRetornarNoContent()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var resultado = await new ClienteController(service.Object).Excluir(Id, CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Excluir_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var resultado = await new ClienteController(service.Object).Excluir(Id, CancellationToken.None);
        Assert.That(resultado, Is.TypeOf<NotFoundResult>());
    }

    private static async Task<IActionResult> ExecutarListar()
    {
        var service = new Mock<IClienteService>();
        service.Setup(x => x.ListarAsync(null, 0, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoPaginadoDto<ClienteDto>([Cliente], 0, 20, 1));
        return await new ClienteController(service.Object).Listar(null, 0, 20, CancellationToken.None);
    }
}
