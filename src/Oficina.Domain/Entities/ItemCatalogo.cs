using Oficina.Domain.Common;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Entities;
public sealed class ItemCatalogo : EntidadeBase
{
    private ItemCatalogo() { }
    public ItemCatalogo(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque) { Nome = nome; Preco = preco; Tipo = tipo; QuantidadeEstoque = quantidadeEstoque; }
    public string Nome { get; private set; } = null!;
    public decimal Preco { get; private set; }
    public TipoItemCatalogo Tipo { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public void Atualizar(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque) { Nome = nome; Preco = preco; Tipo = tipo; QuantidadeEstoque = quantidadeEstoque; AtualizarDataModificacao(); }
}
