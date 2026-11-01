using Oficina.Infra.Data.Context;

namespace Oficina.Api.Configurations;

public static class DatabaseConfiguration
{
    public static string GetConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não configurada.");

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<OficinaDbContext>(
            options => options.UseNpgsql(connectionString, postgres => postgres.MigrationsAssembly("Oficina.Infra.Data.Postgres"))
        );

        return services;
    }
}
