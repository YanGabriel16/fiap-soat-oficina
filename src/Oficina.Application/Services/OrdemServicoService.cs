using Oficina.Domain.Dtos;
using Oficina.Domain.Entities;
using Oficina.Domain.Enums;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class OrdemServicoService(IOrdemServicoRepository repository) : BaseService<OrdemServico, OrdemServicoDto, IOrdemServicoRepository>(repository), IOrdemServicoService
{
    public async Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct)
    {
        var e = new OrdemServico(clienteId, veiculoId);
        await Repository.AdicionarAsync(e, ct);
        return e.Id;
    }
    public async Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct)
    {
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Ordem de serviço não encontrada.");
        e.Atualizar(clienteId, veiculoId, status, valorTotal);
        await Repository.AtualizarAsync(e, ct);
        return e.Id;
    }
    protected override OrdemServicoDto Map(OrdemServico e)
        => new(e.Id, e.ClienteId, e.VeiculoId, e.Status, e.ValorTotal, e.DataCriacao, e.DataUltimaModificacao);
}
