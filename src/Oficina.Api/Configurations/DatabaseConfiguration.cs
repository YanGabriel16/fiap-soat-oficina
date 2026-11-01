namespace Oficina.Api.Configurations;
public static class DatabaseConfiguration
{
    public static string GetConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não configurada.");
}
