namespace Oficina.Application.Tests.Controllers;

public sealed class VeiculoControllerTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static readonly Guid ClienteId = Guid.NewGuid();
    private static VeiculoDto Veiculo => new(Id, ClienteId, "ABC1D23", "Toyota", "Corolla", 2022, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

    [Test]
    public async Task Listar_DeveRetornarOk()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.ListarAsync(null, 0, 20, It.IsAny<CancellationToken>())).ReturnsAsync(new ResultadoPaginadoDto<VeiculoDto>([Veiculo], 0, 20, 1));
        Assert.That(await new VeiculoController(service.Object).Listar(null, 0, 20, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_DeveRetornarOk()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(Veiculo);
        Assert.That(await new VeiculoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync((VeiculoDto?)null);
        Assert.That(await new VeiculoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Criar_DeveRetornarOk()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.CriarAsync(ClienteId, "ABC1D23", "Toyota", "Corolla", 2022, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var request = new CriarVeiculoRequest(ClienteId, "ABC1D23", "Toyota", "Corolla", 2022);
        Assert.That(await new VeiculoController(service.Object).Criar(request, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Atualizar_DeveRetornarOk()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.AtualizarAsync(Id, ClienteId, "ABC1D23", "Toyota", "Corolla", 2022, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var request = new AtualizarVeiculoRequest(ClienteId, "ABC1D23", "Toyota", "Corolla", 2022);
        Assert.That(await new VeiculoController(service.Object).Atualizar(Id, request, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Excluir_QuandoEncontrado_DeveRetornarNoContent()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Assert.That(await new VeiculoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Excluir_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IVeiculoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Assert.That(await new VeiculoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }
}
