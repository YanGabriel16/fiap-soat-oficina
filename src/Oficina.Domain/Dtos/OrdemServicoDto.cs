using Oficina.Domain.Enums;
namespace Oficina.Domain.Dtos;
public sealed record OrdemServicoDto(Guid Id, Guid ClienteId, Guid VeiculoId, StatusOrdemServico Status, decimal ValorTotal, DateTimeOffset DataCriacao, DateTimeOffset DataUltimaModificacao, IReadOnlyList<OrdemServicoItemDto> Itens)
{
    public OrdemServicoDto(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, DateTimeOffset dataCriacao, DateTimeOffset dataUltimaModificacao)
        : this(id, clienteId, veiculoId, status, valorTotal, dataCriacao, dataUltimaModificacao, [])
    {
    }
}
public sealed record OrdemServicoItemDto(Guid ItemCatalogoId, string Nome, int Quantidade, decimal ValorUnitario, decimal ValorTotal);
