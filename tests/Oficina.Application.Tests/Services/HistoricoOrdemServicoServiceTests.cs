namespace Oficina.Application.Tests.Services;

public sealed class HistoricoOrdemServicoServiceTests
{
    [Test]
    public void ClienteSemVinculo_DeveNegarAcessoAoHistorico()
    {
        var repository = new Mock<IOrdemServicoRepository>();
        var clientes = new Mock<IClienteRepository>();
        clientes.Setup(x => x.ObterPorUsuarioAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Cliente?)null);
        var service = new HistoricoOrdemServicoService(repository.Object, clientes.Object);

        Assert.ThrowsAsync<ConflitoException>(() => service.ListarAsync(Guid.NewGuid(), Guid.NewGuid(), true, CancellationToken.None));
        repository.Verify(x => x.ObterHistoricoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ClienteDoProprietario_DeveRetornarHistoricoMapeado()
    {
        var usuarioId = Guid.NewGuid();
        var cliente = new Cliente("Ana", "12345678901", usuarioId);
        var ordem = new OrdemServico(cliente.Id, Guid.NewGuid());
        var evento = new HistoricoOrdemServico(ordem.Id, null, StatusOrdemServico.Recebida, "Recebida", Guid.NewGuid());
        var repository = new Mock<IOrdemServicoRepository>();
        var clientes = new Mock<IClienteRepository>();
        clientes.Setup(x => x.ObterPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);
        repository.Setup(x => x.ObterDetalhadaAsync(ordem.Id, It.IsAny<CancellationToken>())).ReturnsAsync(ordem);
        repository.Setup(x => x.ObterHistoricoAsync(ordem.Id, It.IsAny<CancellationToken>())).ReturnsAsync([evento]);

        var resultado = await new HistoricoOrdemServicoService(repository.Object, clientes.Object).ListarAsync(ordem.Id, usuarioId, true, CancellationToken.None);

        Assert.That(resultado, Has.Count.EqualTo(1));
        Assert.That(resultado[0].OrdemServicoId, Is.EqualTo(ordem.Id));
        Assert.That(resultado[0].NovoStatus, Is.EqualTo(StatusOrdemServico.Recebida));
        Assert.That(resultado[0].Observacao, Is.EqualTo("Recebida"));
    }
}
