using Oficina.Api.Configurations;
using Oficina.Api.Middleware;
using Oficina.Application.Configurations;
namespace Oficina.Api;

public sealed class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<TratamentoExcecoes>();
        services.AddAuthConfiguration();
        services.AddControllersConfiguration();
        services.AddDatabaseConfiguration(Configuration);
        services.AddServiceConfiguration();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfiguration();
        services.AddCorsConfiguration(Configuration);
        services.AddAuthenticationConfiguration(Configuration);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseExceptionHandler();
        app.UseStatusCodePages(); 
        app.UseSwagger(); 
        app.UseSwaggerUI(); 
        app.UseHttpsRedirection(); 
        app.UseRouting(); 
        app.UseCors("Default"); 
        app.UseAuthentication(); 
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
