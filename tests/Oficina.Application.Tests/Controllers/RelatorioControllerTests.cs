namespace Oficina.Application.Tests.Controllers;

public sealed class RelatorioControllerTests
{
    private static readonly DateTimeOffset Inicio = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Fim = new(2026, 1, 31, 23, 59, 59, TimeSpan.Zero);

    [Test]
    public async Task TempoOrdens_DeveRetornarOkEEncaminharPeriodo()
    {
        var service = new Mock<IRelatorioService>();
        var esperado = new RelatorioTempoOrdensResponse(0, 0, null, []);
        service.Setup(x => x.ObterTempoOrdensAsync(Inicio, Fim, It.IsAny<CancellationToken>())).ReturnsAsync(esperado);

        var resultado = await new RelatorioController(service.Object).TempoOrdens(Inicio, Fim, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)resultado).Value, Is.SameAs(esperado));
        service.VerifyAll();
    }

    [Test]
    public async Task Orcamentos_DeveRetornarOk()
    {
        var service = new Mock<IRelatorioService>();
        var esperado = new RelatorioOrcamentosResponse(3, 1, 1, 1, 4.5m, []);
        service.Setup(x => x.ObterOrcamentosAsync(null, null, It.IsAny<CancellationToken>())).ReturnsAsync(esperado);

        var resultado = await new RelatorioController(service.Object).Orcamentos(null, null, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)resultado).Value, Is.SameAs(esperado));
    }

    [Test]
    public async Task StatusOrdens_DeveRetornarOk()
    {
        var service = new Mock<IRelatorioService>();
        var esperado = new[] { new RelatorioStatusOrdemDto(StatusOrdemServico.Recebida, 2) };
        service.Setup(x => x.ObterStatusOrdensAsync(Inicio, null, It.IsAny<CancellationToken>())).ReturnsAsync(esperado);

        var resultado = await new RelatorioController(service.Object).StatusOrdens(Inicio, null, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)resultado).Value, Is.SameAs(esperado));
    }

    [Test]
    public async Task Estoque_DeveEncaminharLimite()
    {
        var service = new Mock<IRelatorioService>();
        service.Setup(x => x.ObterEstoqueAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var resultado = await new RelatorioController(service.Object).Estoque(5, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        service.Verify(x => x.ObterEstoqueAsync(5, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Estoque_ComLimiteInvalido_DeveRetornarBadRequest()
    {
        var service = new Mock<IRelatorioService>();
        service.Setup(x => x.ObterEstoqueAsync(0, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentOutOfRangeException("limite"));
        var controller = new RelatorioController(service.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var resultado = await controller.Estoque(0, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<ObjectResult>());
        Assert.That(((ObjectResult)resultado).StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }
}
