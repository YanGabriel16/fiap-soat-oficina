using Oficina.Domain.Dtos;
using Oficina.Domain.Common;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;

namespace Oficina.Application.Services;

public sealed class HistoricoOrdemServicoService(IOrdemServicoRepository repository, IClienteRepository clientes) : IHistoricoOrdemServicoService
{
    public async Task<IReadOnlyList<HistoricoOrdemServicoDto>> ListarAsync(Guid ordemServicoId, Guid? usuarioId, bool cliente, CancellationToken ct)
    {
        if (cliente && (!usuarioId.HasValue || (await clientes.ObterPorUsuarioAsync(usuarioId.Value, ct)) is not { } c || !await PertenceAoCliente(ordemServicoId, c.Id, ct))) throw new ConflitoException("Acesso negado ao histórico.");
        return (await repository.ObterHistoricoAsync(ordemServicoId, ct)).Select(x => new HistoricoOrdemServicoDto(x.Id, x.OrdemServicoId, x.StatusAnterior, x.NovoStatus, x.Observacao, x.UsuarioId, x.Usuario?.Nome ?? string.Empty, x.DataCriacao, x.Tipo)).ToList();
    }
    private async Task<bool> PertenceAoCliente(Guid ordemId, Guid clienteId, CancellationToken ct) => (await repository.ObterDetalhadaAsync(ordemId, ct))?.ClienteId == clienteId;
}
