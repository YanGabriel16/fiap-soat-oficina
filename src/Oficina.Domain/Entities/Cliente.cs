using Oficina.Domain.Common;
namespace Oficina.Domain.Entities;
public sealed class Cliente : EntidadeBase
{
    private Cliente() { }
    public Cliente(string nome, string documento) { Nome = nome; Documento = documento; }
    public string Nome { get; private set; } = null!;
    public string Documento { get; private set; } = null!;
    public ICollection<Veiculo> Veiculos { get; private set; } = new List<Veiculo>();
    public void Atualizar(string nome, string documento) { Nome = nome; Documento = documento; AtualizarDataModificacao(); }
}
