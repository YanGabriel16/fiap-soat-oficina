using Oficina.Domain.Dtos;
using Oficina.Domain.Common;
using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
namespace Oficina.Application.Services;

public sealed class ClienteService(IClienteRepository repository) : BaseService<Cliente, ClienteDto, IClienteRepository>(repository), IClienteService
{
    public async Task<Guid> CriarAsync(string nome, string documento, CancellationToken ct)
    {
        throw new ArgumentException("UsuarioId é obrigatório para cadastrar um cliente.");
    }
    public async Task<Guid> CriarAsync(string nome, string documento, Guid usuarioId, CancellationToken ct)
    {
        ValidarDocumento(documento);
        
        if (usuarioId == Guid.Empty) 
            throw new ArgumentException("UsuarioId é obrigatório para cadastrar um cliente.");
        
        var e = new Cliente(nome, documento, usuarioId); 
        await Repository.AdicionarAsync(e, ct); 
        return e.Id;
    }
    public async Task<Guid> AtualizarAsync(Guid id, string nome, string documento, CancellationToken ct)
    {
        ValidarDocumento(documento);
        var e = await Repository.ObterPorIdAsync(id, ct) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        
        e.Atualizar(nome, documento);
        await Repository.AtualizarAsync(e, ct);
        
        return e.Id;
    }
    protected override ClienteDto Map(Cliente e)
        => new(e.Id, e.Nome, e.Documento, e.DataCriacao, e.DataUltimaModificacao);
    private static void ValidarDocumento(string documento)
    {
        if (!Validadores.Documento(documento))
            throw new ArgumentException("CPF ou CNPJ inválido.");
    }
}
