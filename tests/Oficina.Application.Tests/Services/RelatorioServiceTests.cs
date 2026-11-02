namespace Oficina.Application.Tests.Services;

public sealed class RelatorioServiceTests
{
    [Test]
    public void PeriodoInvertido_DeveFalharAntesDeConsultarRepositorio()
    {
        var repository = new Mock<IRelatorioRepository>();
        var service = new RelatorioService(repository.Object);

        Assert.ThrowsAsync<ArgumentException>(() => service.ObterTempoOrdensAsync(
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddMinutes(-1),
            CancellationToken.None));
        repository.Verify(x => x.ListarOrdensAsync(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ObterOrcamentos_DeveAgruparDecisoesECalcularDuracao()
    {
        var aprovado = new Orcamento(Guid.NewGuid(), Guid.NewGuid(), 100m);
        aprovado.Decidir(StatusOrcamento.Aprovado);
        var pendente = new Orcamento(Guid.NewGuid(), Guid.NewGuid(), 100m);
        var repository = new Mock<IRelatorioRepository>();
        repository.Setup(x => x.ListarOrcamentosAsync(null, null, It.IsAny<CancellationToken>())).ReturnsAsync([aprovado, pendente]);

        var resultado = await new RelatorioService(repository.Object).ObterOrcamentosAsync(null, null, CancellationToken.None);

        Assert.That(resultado.Total, Is.EqualTo(2));
        Assert.That(resultado.Aprovados, Is.EqualTo(1));
        Assert.That(resultado.Recusados, Is.Zero);
        Assert.That(resultado.Pendentes, Is.EqualTo(1));
        Assert.That(resultado.MediaDuracaoDecisaoHoras, Is.Not.Null);
        Assert.That(resultado.Orcamentos.Single(x => x.OrcamentoId == aprovado.Id).Fim, Is.Not.Null);
    }

    [Test]
    public async Task ObterTempoOrdens_DeveUsarPrimeiroEventoDeFinalizacao()
    {
        var ordemFinalizada = new OrdemServico(Guid.NewGuid(), Guid.NewGuid());
        ordemFinalizada.Historico.Add(new HistoricoOrdemServico(ordemFinalizada.Id, StatusOrdemServico.EmExecucao, StatusOrdemServico.Finalizada, null, Guid.NewGuid()));
        var ordemPendente = new OrdemServico(Guid.NewGuid(), Guid.NewGuid());
        var repository = new Mock<IRelatorioRepository>();
        repository.Setup(x => x.ListarOrdensAsync(null, null, It.IsAny<CancellationToken>())).ReturnsAsync([ordemFinalizada, ordemPendente]);

        var resultado = await new RelatorioService(repository.Object).ObterTempoOrdensAsync(null, null, CancellationToken.None);

        Assert.That(resultado.Total, Is.EqualTo(2));
        Assert.That(resultado.Finalizadas, Is.EqualTo(1));
        Assert.That(resultado.Ordens.Single(x => x.OrdemServicoId == ordemFinalizada.Id).DuracaoHoras, Is.Not.Null);
        Assert.That(resultado.Ordens.Single(x => x.OrdemServicoId == ordemPendente.Id).DuracaoHoras, Is.Null);
    }

    [Test]
    public async Task ObterEstoque_ComLimiteNegativo_DeveFalhar()
    {
        var repository = new Mock<IRelatorioRepository>();

        Assert.ThrowsAsync<ArgumentException>(() => new RelatorioService(repository.Object).ObterEstoqueAsync(-1, CancellationToken.None));
        repository.Verify(x => x.ListarEstoqueAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
