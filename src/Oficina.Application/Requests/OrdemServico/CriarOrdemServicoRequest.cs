namespace Oficina.Application.Requests.OrdemServico;
public sealed record CriarOrdemServicoRequest([Required] Guid ClienteId, [Required] Guid VeiculoId)
{
    [Required, MaxLength(2000)] public string Descricao { get; init; } = string.Empty;
    public IReadOnlyList<ItemOrdemRequest> Itens { get; init; } = [];
    [Range(0, 999999999)] public decimal ValorMaoDeObra { get; init; }
}
public sealed record ItemOrdemRequest([Required] Guid ItemCatalogoId, [Range(1, 100000)] int Quantidade);
