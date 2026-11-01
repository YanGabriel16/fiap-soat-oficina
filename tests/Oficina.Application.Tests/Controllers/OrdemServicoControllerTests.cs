namespace Oficina.Application.Tests.Controllers;

public sealed class OrdemServicoControllerTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static readonly Guid ClienteId = Guid.NewGuid();
    private static readonly Guid VeiculoId = Guid.NewGuid();
    private static OrdemServicoDto Ordem => new(Id, ClienteId, VeiculoId, StatusOrdemServico.Recebida, 100m, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

    [Test]
    public async Task Listar_DeveRetornarOk()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ListarAsync(null, 0, 20, It.IsAny<CancellationToken>())).ReturnsAsync(new ResultadoPaginadoDto<OrdemServicoDto>([Ordem], 0, 20, 1));
        Assert.That(await new OrdemServicoController(service.Object).Listar(null, 0, 20, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_DeveRetornarOk()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(Ordem);
        Assert.That(await new OrdemServicoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task ObterPorId_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ObterPorIdAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync((OrdemServicoDto?)null);
        Assert.That(await new OrdemServicoController(service.Object).ObterPorId(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Criar_DeveRetornarOk()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.CriarAsync(ClienteId, VeiculoId, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        Assert.That(await new OrdemServicoController(service.Object).Criar(new CriarOrdemServicoRequest(ClienteId, VeiculoId), CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Atualizar_DeveRetornarOk()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.AtualizarAsync(Id, ClienteId, VeiculoId, StatusOrdemServico.EmExecucao, 100m, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var request = new AtualizarOrdemServicoRequest(ClienteId, VeiculoId, StatusOrdemServico.EmExecucao, 100m);
        Assert.That(await new OrdemServicoController(service.Object).Atualizar(Id, request, CancellationToken.None), Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task Excluir_QuandoEncontrado_DeveRetornarNoContent()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Assert.That(await new OrdemServicoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task Excluir_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ExcluirAsync(Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Assert.That(await new OrdemServicoController(service.Object).Excluir(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task Minhas_DeveConsultarSomenteClienteAutenticado()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ListarDoClienteAsync(It.IsAny<Guid>(), 0, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoPaginadoDto<OrdemServicoDto>([Ordem], 0, 20, 1));
        var controller = new OrdemServicoController(service.Object) { ControllerContext = ContextoAutenticado() };

        Assert.That(await controller.Minhas(0, 20, CancellationToken.None), Is.TypeOf<OkObjectResult>());
        service.Verify(x => x.ListarDoClienteAsync(It.IsAny<Guid>(), 0, 20, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AtualizarStatus_DeveRetornarNoContent()
    {
        var service = new Mock<IOrdemServicoService>();
        var controller = new OrdemServicoController(service.Object) { ControllerContext = ContextoAutenticado() };

        Assert.That(await controller.AtualizarStatus(Id, new AtualizarStatusOrdemRequest(StatusOrdemServico.EmDiagnostico, "Diagnóstico iniciado"), CancellationToken.None), Is.TypeOf<NoContentResult>());
        service.Verify(x => x.AtualizarStatusAsync(Id, StatusOrdemServico.EmDiagnostico, It.IsAny<Guid>(), "Diagnóstico iniciado", It.IsAny<CancellationToken>()), Times.Once);
    }

    private static ControllerContext ContextoAutenticado()
    {
        var identidade = new System.Security.Claims.ClaimsIdentity([
            new System.Security.Claims.Claim("sub", Guid.NewGuid().ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Cliente")], "test");
        return new ControllerContext { HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(identidade) } };
    }
}
