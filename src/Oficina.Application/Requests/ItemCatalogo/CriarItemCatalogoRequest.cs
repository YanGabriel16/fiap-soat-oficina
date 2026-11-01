using System.ComponentModel.DataAnnotations;
using Oficina.Domain.Enums;
namespace Oficina.Application.Requests.ItemCatalogo;
public sealed record CriarItemCatalogoRequest([Required, MaxLength(160)] string Nome, [Range(0, 999999999)] decimal Preco, TipoItemCatalogo Tipo, [Range(0, int.MaxValue)] int QuantidadeEstoque);
