using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomSolutionName.Api.Configurations;

public static class AuthenticationProviderRegistries
{
    public static void RegisterAuthenticationProviders(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthentication()
            .AddCookie("Cookies");
    }
}