using Oficina.Domain.Common;

namespace Oficina.Domain.Entities;

public sealed class OrcamentoItem : EntidadeBase
{
    private OrcamentoItem() { }
    public OrcamentoItem(Guid orcamentoId, Guid itemCatalogoId, int quantidade, decimal valorUnitario)
    {
        if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
        OrcamentoId = orcamentoId; ItemCatalogoId = itemCatalogoId; Quantidade = quantidade; ValorUnitario = valorUnitario;
    }
    public Guid OrcamentoId { get; private set; }
    public Guid ItemCatalogoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal => Quantidade * ValorUnitario;
    public ItemCatalogo ItemCatalogo { get; private set; } = null!;
}
