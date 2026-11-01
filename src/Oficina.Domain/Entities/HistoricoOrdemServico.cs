using Oficina.Domain.Common;
using Oficina.Domain.Enums;

namespace Oficina.Domain.Entities;

public sealed class HistoricoOrdemServico : EntidadeBase
{
    private HistoricoOrdemServico() { }
    public HistoricoOrdemServico(Guid ordemServicoId, StatusOrdemServico? statusAnterior, StatusOrdemServico novoStatus, string? observacao, Guid usuarioId, TipoHistoricoOrdem tipo = TipoHistoricoOrdem.OrdemServico)
    {
        OrdemServicoId = ordemServicoId; StatusAnterior = statusAnterior; NovoStatus = novoStatus; Observacao = observacao; UsuarioId = usuarioId; Tipo = tipo;
    }
    public Guid OrdemServicoId { get; private set; }
    public StatusOrdemServico? StatusAnterior { get; private set; }
    public StatusOrdemServico NovoStatus { get; private set; }
    public string? Observacao { get; private set; }
    public Guid UsuarioId { get; private set; }
    public TipoHistoricoOrdem Tipo { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public OrdemServico OrdemServico { get; private set; } = null!;
}
