using Oficina.Api.Configurations;
using Oficina.Api.Middleware;
using Oficina.Application.Configurations;
using Oficina.Domain.Dtos;
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
        app.UseStatusCodePages(async statusContext =>
        {
            var response = statusContext.HttpContext.Response;
            if (response.HasStarted || response.ContentLength is not null)
                return;

            var mensagem = response.StatusCode switch
            {
                StatusCodes.Status401Unauthorized => "Autenticação necessária.",
                StatusCodes.Status403Forbidden => "Acesso negado.",
                StatusCodes.Status404NotFound => "Recurso não encontrado.",
                _ => "Não foi possível processar a requisição."
            };

            response.ContentType = "application/json; charset=utf-8";
            await response.WriteAsJsonAsync(new ErroResponse(response.StatusCode, mensagem, statusContext.HttpContext.Request.Path));
        }); 
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
