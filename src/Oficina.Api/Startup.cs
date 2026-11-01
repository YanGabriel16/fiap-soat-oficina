using Oficina.Api.Configurations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Oficina.Application.Services;
using Oficina.Application.Controllers;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
using Oficina.Infra.Data.Context;
using Oficina.Infra.Data.Repositories;
using Oficina.Api.Middleware;
namespace Oficina.Api;
public sealed class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<TratamentoExcecoes>();
        services.AddControllers().AddApplicationPart(typeof(ClienteController).Assembly).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddDbContext<OficinaDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), postgres => postgres.MigrationsAssembly("Oficina.Infra.Data.Postgres")));
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IItemCatalogoRepository, ItemCatalogoRepository>();
        services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IVeiculoService, VeiculoService>();
        services.AddScoped<IItemCatalogoService, ItemCatalogoService>();
        services.AddScoped<IOrdemServicoService, OrdemServicoService>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfiguration();
        services.AddCorsConfiguration(Configuration);
        services.AddAuthenticationConfiguration(Configuration);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
        app.UseExceptionHandler(); app.UseStatusCodePages(); app.UseSwagger(); app.UseSwaggerUI(); app.UseHttpsRedirection(); app.UseRouting(); app.UseCors("Default"); app.UseAuthentication(); app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
