using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Application.Ports.Driven.MessageBroker;
using CustomSolutionName.Infrastructure.DataAccess;
using CustomSolutionName.Infrastructure.DataAccess.MultiTenancy;
using CustomSolutionName.Infrastructure.DataAccess.RepositoryAdaptors;
using CustomSolutionName.Infrastructure.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomSolutionName.Infrastructure;

public static class DependencyInjection
{
    public static IServiceProvider ServiceProvider = null!;
    
    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("__CONNECTIONSTRING__");

        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();
            });

        // Repositories
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserIdProvider, UserIdProvider>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IBlacklistedTokenRepository, BlacklistedTokenRepository>();
        
        services.AddTransient<IMessageSenderGateway, MessageSenderGatewayMock>();
        
        // Cloud-Native Services
        if (environment.IsDevelopment())
        {
            // DEV ENVIRONMENT specific settings
            // services.AddTransient<IMessageSenderGateway, MessageSenderGatewayMock>();
        }
        else
        {
            // PROD ENVIRONMENT specific settings
            // services.AddTransient<IMessageSenderGateway, AzureBusMessageSenderGateway>();
        }

        ServiceProvider = services.BuildServiceProvider();
        
        return services;
    }
}