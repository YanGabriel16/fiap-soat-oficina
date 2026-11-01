using Oficina.Domain.Common;
namespace Oficina.Domain.Entities;
public sealed class Veiculo : EntidadeBase
{
    private Veiculo() { }
    public Veiculo(Guid clienteId, string placa, string marca, string modelo, int ano) { ClienteId = clienteId; Placa = placa; Marca = marca; Modelo = modelo; Ano = ano; }
    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; } = null!;
    public string Placa { get; private set; } = null!;
    public string Marca { get; private set; } = null!;
    public string Modelo { get; private set; } = null!;
    public int Ano { get; private set; }
    public void Atualizar(Guid clienteId, string placa, string marca, string modelo, int ano) { ClienteId = clienteId; Placa = placa; Marca = marca; Modelo = modelo; Ano = ano; AtualizarDataModificacao(); }
}
