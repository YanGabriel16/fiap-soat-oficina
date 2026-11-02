using Oficina.Domain.Dtos;
using Oficina.Domain.Enums;
namespace Oficina.Domain.Interfaces.Services;

public interface IItemCatalogoService : IBaseService<ItemCatalogoDto>
{
    Task<Guid> CriarAsync(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct); 
    Task<Guid> AtualizarAsync(Guid id, string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque, CancellationToken ct);
    Task<Guid> AdicionarEstoqueAsync(Guid id, int quantidade, CancellationToken ct);
    Task<IReadOnlyList<HistoricoItemCatalogoDto>> ListarHistoricoAsync(Guid id, CancellationToken ct);
}
