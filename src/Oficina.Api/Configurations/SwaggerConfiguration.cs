namespace Oficina.Api.Configurations;
public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services) 
        => services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo { Title = "Oficina API", Version = "v1" }));
}
