using Oficina.Domain.Common;
namespace Oficina.Domain.Entities;
public sealed class Cliente : EntidadeBase
{
    private Cliente() { }
    public Cliente(string nome, string documento, Guid usuarioId = default, string? telefone = null)
    {
        Nome = nome;
        Documento = documento;
        UsuarioId = usuarioId;
        Telefone = telefone;
    }
    public string Nome { get; private set; } = null!;
    public string Documento { get; private set; } = null!;
    public string? Telefone { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public ICollection<Veiculo> Veiculos { get; private set; } = new List<Veiculo>();
    public void Atualizar(string nome, string documento, string? telefone = null)
    {
        Nome = nome;
        Documento = documento;
        Telefone = telefone;
        AtualizarDataModificacao();
    }
}
