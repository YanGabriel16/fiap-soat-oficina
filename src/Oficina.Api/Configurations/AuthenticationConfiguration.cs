using Microsoft.AspNetCore.Identity;
using Oficina.Domain.Entities;

namespace Oficina.Api.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
        return services;
    }
    
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"] ?? "development-only-key-change-me-32-chars";
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), ValidateIssuer = false, ValidateAudience = false, ValidateLifetime = true });
        services.AddAuthorization();
        
        return services;
    }
}
