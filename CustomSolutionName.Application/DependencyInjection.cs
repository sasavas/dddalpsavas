using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSolutionName.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly());
            });
        
        return services;
    }
}