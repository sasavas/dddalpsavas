using System.Text.Json.Serialization;
using CustomSolutionName.Api.Authentication;
using CustomSolutionName.Api.Configurations;
using CustomSolutionName.Api.MiddleWares;
using CustomSolutionName.Api.Utils;
using CustomSolutionName.Application;
using CustomSolutionName.Infrastructure;
using CustomSolutionName.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
    );

builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables();


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
    serverOptions.ListenAnyIP(5001, listenOptions => { listenOptions.UseHttps(); });
});


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthorization();

builder.Services.AddInfrastructureDependencies(builder.Configuration, builder.Environment);
builder.Services.AddApplicationDependencies();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddRateLimitingConfiguration();

builder.Services.RegisterAuthenticationProviders(builder.Configuration);

builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddTransient<ApiEndpointHitLoggerMiddleware>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
else if (app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
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

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseAuthentication();
app.UseMiddleware<BlacklistedTokenCheckMiddleware>();
app.UseAuthorization();

app.UseRateLimiter();

app.UseMiddleware<ApiEndpointHitLoggerMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

// DO NOT DELETE, hack to access in tests etc.
public partial class Program
{
}