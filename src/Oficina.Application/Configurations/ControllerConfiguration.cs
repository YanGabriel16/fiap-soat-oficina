using System.Text.Json.Serialization;
using Oficina.Domain.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Oficina.Application.Controllers;

namespace Oficina.Application.Configurations;

public static class ControllerConfiguration
{
    public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(ClienteController).Assembly)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var erros = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

                var resposta = new ErroResponse(
                    StatusCodes.Status400BadRequest,
                    "Dados inválidos.",
                    context.HttpContext.Request.Path,
                    erros);

                return new BadRequestObjectResult(resposta);
            };
        });

        return services;
    }
}
