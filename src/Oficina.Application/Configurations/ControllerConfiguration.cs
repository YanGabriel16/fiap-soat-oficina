using System.Text.Json.Serialization;
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

        return services;
    }
}
