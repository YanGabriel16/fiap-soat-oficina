using Oficina.Domain.Common;
using Oficina.Domain.Enums;

namespace Oficina.Domain.Entities;

public sealed class Orcamento : EntidadeBase
{
    private Orcamento() { }
    public Orcamento(Guid ordemServicoId, Guid clienteId, decimal valorMaoDeObra)
    {
        OrdemServicoId = ordemServicoId; ClienteId = clienteId; ValorMaoDeObra = valorMaoDeObra; Status = StatusOrcamento.Pendente;
    }
    public Guid OrdemServicoId { get; private set; }
    public Guid ClienteId { get; private set; }
    public decimal ValorMaoDeObra { get; private set; }
    public decimal ValorTotal => Itens.Sum(x => x.ValorTotal) + ValorMaoDeObra;
    public StatusOrcamento Status { get; private set; }
    public DateTimeOffset? DataDecisao { get; private set; }
    public OrdemServico OrdemServico { get; private set; } = null!;
    public Cliente Cliente { get; private set; } = null!;
    public ICollection<OrcamentoItem> Itens { get; private set; } = new List<OrcamentoItem>();

    public void AdicionarItem(Guid itemCatalogoId, int quantidade, decimal valorUnitario) => Itens.Add(new OrcamentoItem(Id, itemCatalogoId, quantidade, valorUnitario));
    public void Decidir(StatusOrcamento status)
    {
        if (Status != StatusOrcamento.Pendente) throw new ConflitoException("O orçamento já foi decidido.");
        if (status is not (StatusOrcamento.Aprovado or StatusOrcamento.Recusado)) throw new ArgumentException("Decisão inválida.");
        Status = status; DataDecisao = DateTimeOffset.UtcNow; AtualizarDataModificacao();
    }
}
