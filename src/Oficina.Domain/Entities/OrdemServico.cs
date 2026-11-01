using Oficina.Domain.Common;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Entities;
public sealed class OrdemServico : EntidadeBase
{
    private OrdemServico() { }
    public OrdemServico(Guid clienteId, Guid veiculoId, Guid atendenteId = default, string descricao = "") { ClienteId = clienteId; VeiculoId = veiculoId; AtendenteId = atendenteId; Descricao = descricao; Status = StatusOrdemServico.Recebida; }
    public Guid ClienteId { get; private set; }
    public Guid VeiculoId { get; private set; }
    public StatusOrdemServico Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public Guid AtendenteId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public decimal ValorMaoDeObra { get; private set; }
    public Cliente Cliente { get; private set; } = null!;
    public Usuario Atendente { get; private set; } = null!;
    public ICollection<OrdemServicoItem> Itens { get; private set; } = new List<OrdemServicoItem>();
    public Orcamento? Orcamento { get; private set; }
    public ICollection<HistoricoOrdemServico> Historico { get; private set; } = new List<HistoricoOrdemServico>();
    public void Atualizar(Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal) { ClienteId = clienteId; VeiculoId = veiculoId; Status = status; ValorTotal = valorTotal; AtualizarDataModificacao(); }
    public void DefinirOrcamento(decimal valorMaoDeObra, decimal valorTotal) { ValorMaoDeObra = valorMaoDeObra; ValorTotal = valorTotal; AtualizarDataModificacao(); }
    public void AlterarStatus(StatusOrdemServico novoStatus)
    {
        if (Status == novoStatus) throw new ConflitoException("A ordem já está nesse status.");
        var valido = (Status, novoStatus) switch
        {
            (StatusOrdemServico.Recebida, StatusOrdemServico.EmDiagnostico) => true,
            (StatusOrdemServico.Recebida, StatusOrdemServico.AguardandoAprovacao) => true,
            (StatusOrdemServico.EmDiagnostico, StatusOrdemServico.AguardandoAprovacao) => true,
            (StatusOrdemServico.AguardandoAprovacao, StatusOrdemServico.EmExecucao) => true,
            (StatusOrdemServico.AguardandoAprovacao, StatusOrdemServico.Recebida) => true,
            (StatusOrdemServico.EmExecucao, StatusOrdemServico.Finalizada) => true,
            (StatusOrdemServico.Finalizada, StatusOrdemServico.Entregue) => true,
            _ => false
        };
        if (!valido) throw new ConflitoException("Transição de status inválida.");
        Status = novoStatus; AtualizarDataModificacao();
    }
}
