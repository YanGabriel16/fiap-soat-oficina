using Oficina.Domain.Common;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Entities;
public sealed class OrdemServico : EntidadeBase
{
    private OrdemServico() { }
    public OrdemServico(Guid clienteId, Guid veiculoId) { ClienteId = clienteId; VeiculoId = veiculoId; Status = StatusOrdemServico.Recebida; }
    public Guid ClienteId { get; private set; }
    public Guid VeiculoId { get; private set; }
    public StatusOrdemServico Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public void Atualizar(Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal) { ClienteId = clienteId; VeiculoId = veiculoId; Status = status; ValorTotal = valorTotal; AtualizarDataModificacao(); }
}
