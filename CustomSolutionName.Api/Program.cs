using System.Text.Json.Serialization;
using CustomSolutionName.Api.Authentication;
using CustomSolutionName.Api.DependencyConfigurations;
using CustomSolutionName.Api.MiddleWares;
using CustomSolutionName.Api.Utils;
using CustomSolutionName.Application;
using CustomSolutionName.Infrastructure;
using CustomSolutionName.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables();

builder.Host
    .UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
    );

builder.Services
    .AddLogging()
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .Configure<FormOptions>(x =>
    {
        x.ValueLengthLimit = int.MaxValue;
        x.MultipartBodyLengthLimit = int.MaxValue;
        x.MultipartHeadersLengthLimit = int.MaxValue;
    })
    .AddEndpointsApiExplorer();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
    serverOptions.ListenAnyIP(5001, listenOptions => { listenOptions.UseHttps(); });
});

builder.Services
    .Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"))
    .ConfigureOptions<JwtBearerOptionsConfiguration>()
    .AddAuthorization()
    .AddSwaggerConfiguration()
    .AddRateLimitingConfiguration()
    .AddAuthenticationProviders(builder.Configuration)
    .AddInfrastructureDependencies(builder.Configuration, builder.Environment)
    .AddApplicationDependencies()
    .AddTransient<ExceptionMiddleware>()
    .AddTransient<ApiEndpointHitLoggerMiddleware>()
    .AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    DatabaseSeeder.SeedDatabase(context);
}

app
    .UseHttpsRedirection()
    .UseCors(corsPolicyBuilder =>
    {
        //TODO:prod DO NOT FORGET to enable only trusted origins
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    })
    .UseMiddleware<ExceptionMiddleware>()
    .UseMiddleware<ApiEndpointHitLoggerMiddleware>()
    .UseAuthentication()
    .UseMiddleware<BlacklistedTokenCheckMiddleware>()
    .UseAuthorization()
    .UseRateLimiter();

app.MapControllers();

app.Run();

// DO NOT DELETE, hack to access in tests etc.
public partial class Program
{
}