using Oficina.Domain.Common;

namespace Oficina.Domain.Entities;

public sealed class OrdemServicoItem : EntidadeBase
{
    private OrdemServicoItem() { }
    public OrdemServicoItem(Guid ordemServicoId, Guid itemCatalogoId, int quantidade, decimal valorUnitario)
    {
        if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
        if (valorUnitario < 0) throw new ArgumentException("O valor unitário não pode ser negativo.");
        OrdemServicoId = ordemServicoId; ItemCatalogoId = itemCatalogoId; Quantidade = quantidade; ValorUnitario = valorUnitario;
    }
    public Guid OrdemServicoId { get; private set; }
    public Guid ItemCatalogoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal => Quantidade * ValorUnitario;
    public ItemCatalogo ItemCatalogo { get; private set; } = null!;
}
