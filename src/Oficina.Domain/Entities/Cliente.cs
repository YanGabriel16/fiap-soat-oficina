using Oficina.Domain.Common;
namespace Oficina.Domain.Entities;
public sealed class Cliente : EntidadeBase
{
    private Cliente() { }
    public Cliente(string nome, string documento, Guid usuarioId = default) { Nome = nome; Documento = documento; UsuarioId = usuarioId; }
    public string Nome { get; private set; } = null!;
    public string Documento { get; private set; } = null!;
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public ICollection<Veiculo> Veiculos { get; private set; } = new List<Veiculo>();
    public void Atualizar(string nome, string documento) { Nome = nome; Documento = documento; AtualizarDataModificacao(); }
}
