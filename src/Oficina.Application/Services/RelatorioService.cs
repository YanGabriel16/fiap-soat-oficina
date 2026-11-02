using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public sealed class RelatorioService(IRelatorioRepository repository) : IRelatorioService
{
    public async Task<RelatorioTempoOrdensResponse> ObterTempoOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct)
    {
        ValidarPeriodo(inicio, fim);
        var ordens = await repository.ListarOrdensAsync(inicio, fim, ct);
        var itens = ordens.Select(ordem =>
        {
            var dataFim = ordem.Historico
                .Where(x => x.NovoStatus is StatusOrdemServico.Finalizada or StatusOrdemServico.Entregue)
                .OrderBy(x => x.DataCriacao)
                .Select(x => (DateTimeOffset?)x.DataCriacao)
                .FirstOrDefault();

            return new RelatorioTempoOrdemDto(
                ordem.Id,
                ordem.Status,
                ordem.DataCriacao,
                dataFim,
                CalcularHoras(ordem.DataCriacao, dataFim));
        }).ToList();

        return new(
            itens.Count,
            itens.Count(x => x.Fim.HasValue),
            Media(itens.Where(x => x.DuracaoHoras.HasValue).Select(x => x.DuracaoHoras!.Value)),
            itens);
    }

    public async Task<RelatorioOrcamentosResponse> ObterOrcamentosAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct)
    {
        ValidarPeriodo(inicio, fim);
        var orcamentos = await repository.ListarOrcamentosAsync(inicio, fim, ct);
        var itens = orcamentos.Select(orcamento => new RelatorioOrcamentoDto(
            orcamento.Id,
            orcamento.OrdemServicoId,
            orcamento.Status,
            orcamento.DataCriacao,
            orcamento.DataDecisao,
            CalcularHoras(orcamento.DataCriacao, orcamento.DataDecisao))).ToList();

        return new(
            itens.Count,
            itens.Count(x => x.Status == StatusOrcamento.Aprovado),
            itens.Count(x => x.Status == StatusOrcamento.Recusado),
            itens.Count(x => x.Status == StatusOrcamento.Pendente),
            Media(itens.Where(x => x.DuracaoHoras.HasValue).Select(x => x.DuracaoHoras!.Value)),
            itens);
    }

    public async Task<IReadOnlyList<RelatorioStatusOrdemDto>> ObterStatusOrdensAsync(DateTimeOffset? inicio, DateTimeOffset? fim, CancellationToken ct)
    {
        ValidarPeriodo(inicio, fim);
        var ordens = await repository.ListarOrdensAsync(inicio, fim, ct);

        return Enum.GetValues<StatusOrdemServico>()
            .Select(status => new RelatorioStatusOrdemDto(status, ordens.Count(x => x.Status == status)))
            .ToList();
    }

    public async Task<IReadOnlyList<RelatorioEstoqueDto>> ObterEstoqueAsync(int limite, CancellationToken ct)
    {
        if (limite < 0) throw new ArgumentException("O limite do relatório de estoque não pode ser negativo.");

        var itens = await repository.ListarEstoqueAsync(limite, ct);
        return itens.Select(x => new RelatorioEstoqueDto(x.Id, x.Nome, x.Tipo, x.QuantidadeEstoque, x.Preco)).ToList();
    }

    private static decimal? CalcularHoras(DateTimeOffset inicio, DateTimeOffset? fim)
        => fim.HasValue ? Math.Round((decimal)(fim.Value - inicio).TotalHours, 2) : null;

    private static decimal? Media(IEnumerable<decimal> valores)
    {
        var lista = valores.ToList();
        return lista.Count == 0 ? null : Math.Round(lista.Average(), 2);
    }

    private static void ValidarPeriodo(DateTimeOffset? inicio, DateTimeOffset? fim)
    {
        if (inicio.HasValue && fim.HasValue && inicio > fim)
            throw new ArgumentException("A data inicial não pode ser maior que a data final.");
    }
}
