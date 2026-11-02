using Oficina.Domain.Entities;
namespace Oficina.Infra.Data.Context;
public sealed class OficinaDbContext(DbContextOptions<OficinaDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<ItemCatalogo> ItensCatalogo => Set<ItemCatalogo>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<Orcamento> Orcamentos => Set<Orcamento>();
    public DbSet<OrcamentoItem> OrcamentoItens => Set<OrcamentoItem>();
    public DbSet<OrdemServicoItem> OrdemServicoItens => Set<OrdemServicoItem>();
    public DbSet<HistoricoOrdemServico> HistoricosOrdensServico => Set<HistoricoOrdemServico>();
    public DbSet<HistoricoItemCatalogo> HistoricosItensCatalogo => Set<HistoricoItemCatalogo>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(OficinaDbContext).Assembly);
}
