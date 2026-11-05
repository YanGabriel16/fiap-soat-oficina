using Oficina.Domain.Entities;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Infra.Data.Context;
namespace Oficina.Infra.Data.Repositories;
public sealed class ClienteRepository(OficinaDbContext db) : BaseRepository<Cliente>(db), IClienteRepository
{
    public Task<Cliente?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken ct)
        => (from cliente in Db.Clientes
            where cliente.UsuarioId == usuarioId
            select cliente).SingleOrDefaultAsync(ct);

    public Task<Cliente?> ObterDetalhadoAsync(Guid id, CancellationToken ct)
        => Db.Clientes
            .Include(cliente => cliente.Usuario)
            .SingleOrDefaultAsync(cliente => cliente.Id == id, ct);

    protected override IQueryable<Cliente> AplicarFiltro(IQueryable<Cliente> query, string? term) 
        => from cliente in query
           where string.IsNullOrEmpty(term)
                 || EF.Functions.Like(cliente.Nome.ToLower(), $"%{term!.ToLower()}%")
                 || EF.Functions.Like(cliente.Documento, $"%{term!}%")
           select cliente;
           
    protected override IQueryable<Cliente> AplicarOrdenacao(IQueryable<Cliente> query) 
        => from cliente in query
           orderby cliente.Nome
           select cliente;
}
