namespace Oficina.Application.Requests.ItemCatalogo;

public sealed record AdicionarEstoqueRequest([Range(1, int.MaxValue)] int Quantidade);
