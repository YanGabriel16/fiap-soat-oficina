using Oficina.Domain.Enums;

namespace Oficina.Domain.Entities;

public sealed class HistoricoItemCatalogo
{
    private HistoricoItemCatalogo() { }

    public HistoricoItemCatalogo(ItemCatalogo item, AcaoHistoricoItemCatalogo acao)
    {
        ItemCatalogoId = item.Id;
        Acao = acao;
        Nome = item.Nome;
        Preco = item.Preco;
        Tipo = item.Tipo;
        QuantidadeEstoque = item.QuantidadeEstoque;
        DataEvento = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ItemCatalogoId { get; private set; }
    public AcaoHistoricoItemCatalogo Acao { get; private set; }
    public string Nome { get; private set; } = null!;
    public decimal Preco { get; private set; }
    public TipoItemCatalogo Tipo { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public DateTimeOffset DataEvento { get; private set; }
}
