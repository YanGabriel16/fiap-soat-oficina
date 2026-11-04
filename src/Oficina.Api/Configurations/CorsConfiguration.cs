namespace Oficina.Api.Configurations;
public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration) 
        => services.AddCors(options => options.AddPolicy("Default", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
}
