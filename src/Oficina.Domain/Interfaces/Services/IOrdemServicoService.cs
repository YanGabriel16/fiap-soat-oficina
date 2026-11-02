using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Interfaces.Services;

public interface IOrdemServicoService : IBaseService<OrdemServicoDto>
{
    Task<Guid> CriarAsync(Guid clienteId, Guid veiculoId, CancellationToken ct);
    Task<Guid> AtualizarAsync(Guid id, Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal, CancellationToken ct);
    Task<Guid> CriarComOrcamentoAsync(Guid clienteId, Guid veiculoId, Guid atendenteId, string descricao, IReadOnlyList<(Guid ItemCatalogoId, int Quantidade)> itens, decimal valorMaoDeObra, CancellationToken ct);
    Task AtualizarStatusAsync(Guid id, StatusOrdemServico novoStatus, Guid usuarioId, string? observacao, CancellationToken ct);
    Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarDoClienteAsync(Guid usuarioId, int offset, int limit, CancellationToken ct);
    Task<ResultadoPaginadoDto<OrdemServicoDto>> ListarPorNomeClienteAsync(string nome, int offset, int limit, CancellationToken ct);
}
