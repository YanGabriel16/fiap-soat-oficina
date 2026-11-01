using Microsoft.Extensions.DependencyInjection;
using Oficina.Application.Services;
using Oficina.Domain.Interfaces.Repositories;
using Oficina.Domain.Interfaces.Services;
using Oficina.Infra.Data.Repositories;

namespace Oficina.Application.Configurations;

public static class ServiceConfiguration
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
    {
        #region Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IVeiculoService, VeiculoService>();
        services.AddScoped<IItemCatalogoService, ItemCatalogoService>();
        services.AddScoped<IOrdemServicoService, OrdemServicoService>();
        #endregion

        #region Repositories
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IItemCatalogoRepository, ItemCatalogoRepository>();
        services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        #endregion
        
        return services;
    }
}
