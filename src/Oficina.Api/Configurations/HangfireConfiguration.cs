using Hangfire;
using Hangfire.PostgreSql;

namespace Oficina.Api.Configurations;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString();

        services.AddHangfire(hangfire => hangfire
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(storage => storage.UseNpgsqlConnection(connectionString)));
        services.AddHangfireServer();

        return services;
    }
}
