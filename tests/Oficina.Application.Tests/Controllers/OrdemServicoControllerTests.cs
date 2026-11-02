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
    public async Task Criar_ComUsuarioAutenticado_DeveCriarComOrcamentoERetornarCreated()
    {
        var atendenteId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.CriarComOrcamentoAsync(
                ClienteId, VeiculoId, atendenteId, "Revisão",
                It.Is<IReadOnlyList<(Guid ItemCatalogoId, int Quantidade)>>(itens => itens.Count == 1 && itens[0].ItemCatalogoId == itemId && itens[0].Quantidade == 2),
                150m, It.IsAny<CancellationToken>())).ReturnsAsync(Id);
        var controller = new OrdemServicoController(service.Object) { ControllerContext = ContextoAutenticado(atendenteId, "Atendente") };
        var request = new CriarOrdemServicoRequest(ClienteId, VeiculoId)
        {
            Descricao = "Revisão",
            ValorMaoDeObra = 150m,
            Itens = [new ItemOrdemRequest(itemId, 2)]
        };

        var resultado = await controller.Criar(request, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<ObjectResult>());
        Assert.That(((ObjectResult)resultado).StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(((ObjectResult)resultado).Value, Is.EqualTo(Id));
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
        var usuarioId = Guid.NewGuid();
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ListarDoClienteAsync(usuarioId, 0, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoPaginadoDto<OrdemServicoDto>([Ordem], 0, 20, 1));
        var controller = new OrdemServicoController(service.Object) { ControllerContext = ContextoAutenticado(usuarioId) };

        Assert.That(await controller.Minhas(0, 20, CancellationToken.None), Is.TypeOf<OkObjectResult>());
        service.Verify(x => x.ListarDoClienteAsync(usuarioId, 0, 20, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task PorCliente_DeveEncaminharNomeEPaginacao()
    {
        var service = new Mock<IOrdemServicoService>();
        service.Setup(x => x.ListarPorNomeClienteAsync("Ana", 10, 5, It.IsAny<CancellationToken>())).ReturnsAsync(new ResultadoPaginadoDto<OrdemServicoDto>([], 10, 5, 0));

        var resultado = await new OrdemServicoController(service.Object).PorCliente("Ana", 10, 5, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        service.Verify(x => x.ListarPorNomeClienteAsync("Ana", 10, 5, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AtualizarStatus_DeveRetornarNoContent()
    {
        var service = new Mock<IOrdemServicoService>();
        var controller = new OrdemServicoController(service.Object) { ControllerContext = ContextoAutenticado() };

        Assert.That(await controller.AtualizarStatus(Id, new AtualizarStatusOrdemRequest(StatusOrdemServico.EmDiagnostico, "Diagnóstico iniciado"), CancellationToken.None), Is.TypeOf<NoContentResult>());
        service.Verify(x => x.AtualizarStatusAsync(Id, StatusOrdemServico.EmDiagnostico, It.IsAny<Guid>(), "Diagnóstico iniciado", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Historico_DeveRetornarOkEAplicarUsuarioAtual()
    {
        var usuarioId = Guid.NewGuid();
        var service = new Mock<IOrdemServicoService>();
        var historicoService = new Mock<IHistoricoOrdemServicoService>();
        var historico = new[] { new HistoricoOrdemServicoDto(Guid.NewGuid(), Id, null, StatusOrdemServico.Recebida, null, usuarioId, "Atendente", DateTimeOffset.UtcNow, TipoHistoricoOrdem.OrdemServico) };
        historicoService.Setup(x => x.ListarAsync(Id, usuarioId, false, It.IsAny<CancellationToken>())).ReturnsAsync(historico);
        var controller = new OrdemServicoController(service.Object, historicoService: historicoService.Object) { ControllerContext = ContextoAutenticado(usuarioId, "Atendente") };

        var resultado = await controller.Historico(Id, CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)resultado).Value, Is.SameAs(historico));
    }

    [Test]
    public async Task Orcamento_QuandoNaoEncontrado_DeveRetornarNotFound()
    {
        var service = new Mock<IOrdemServicoService>();
        var orcamentoService = new Mock<IOrcamentoService>();
        orcamentoService.Setup(x => x.ObterPorOrdemAsync(Id, It.IsAny<Guid>(), false, It.IsAny<CancellationToken>())).ReturnsAsync((OrcamentoDto?)null);
        var controller = new OrdemServicoController(service.Object, orcamentoService.Object) { ControllerContext = ContextoAutenticado(Guid.NewGuid(), "Atendente") };

        Assert.That(await controller.Orcamento(Id, CancellationToken.None), Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DecidirOrcamento_DeveEncaminharDecisaoERetornarOk()
    {
        var usuarioId = Guid.NewGuid();
        var service = new Mock<IOrdemServicoService>();
        var orcamentoService = new Mock<IOrcamentoService>();
        var controller = new OrdemServicoController(service.Object, orcamentoService.Object) { ControllerContext = ContextoAutenticado(usuarioId, "Cliente") };

        var resultado = await controller.DecidirOrcamento(Id, new DecidirOrcamentoRequest(StatusOrcamento.Recusado), CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkResult>());
        orcamentoService.Verify(x => x.DecidirAsync(Id, usuarioId, StatusOrcamento.Recusado, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static ControllerContext ContextoAutenticado(Guid? usuarioId = null, string role = "Cliente")
    {
        var id = (usuarioId ?? Guid.NewGuid()).ToString();
        var identidade = new System.Security.Claims.ClaimsIdentity([
            new System.Security.Claims.Claim("sub", id),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, id),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)], "test");
        return new ControllerContext { HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(identidade) } };
    }
}
