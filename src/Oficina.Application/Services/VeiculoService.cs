using Oficina.Domain.Dtos;
using Oficina.Domain.Common;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class VeiculoService(IVeiculoRepository repository) : BaseService<Veiculo, VeiculoDto, IVeiculoRepository>(repository), IVeiculoService
{
    public async Task<Guid> CriarAsync(Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct)
    {
        ValidarPlaca(placa);
        var e = new Veiculo(clienteId, placa.ToUpperInvariant(), marca, modelo, ano);
        await Repository.AdicionarAsync(e, ct);
        return e.Id;
    }
    public async Task<Guid> AtualizarAsync(Guid id, Guid clienteId, string placa, string marca, string modelo, int ano, CancellationToken ct)
    {
        ValidarPlaca(placa);
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Veículo não encontrado.");
        e.Atualizar(clienteId, placa.ToUpperInvariant(), marca, modelo, ano);
        await Repository.AtualizarAsync(e, ct);
        return e.Id;
    }
    protected override VeiculoDto Map(Veiculo e)
        => new(e.Id, e.ClienteId, e.Placa, e.Marca, e.Modelo, e.Ano, e.DataCriacao, e.DataUltimaModificacao);
    private static void ValidarPlaca(string placa)
    {
        if (!Validadores.Placa(placa)) throw new ArgumentException("Placa de veículo inválida.");
    }
}
