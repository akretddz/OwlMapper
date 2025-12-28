using OwlMapper.Bootstrapper.Models;
using OwlMapper.Bootstrapper.Modules;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting OwlMapper Bootstrapper");

    builder.Services.AddOpenApi();

    var applicationInfo = new ApplicationInfo();
    builder.Configuration.GetSection("ApplicationInfo").Bind(applicationInfo);
    
    var envIdentifier = Environment.GetEnvironmentVariable("ApplicationIdentifier");
    var envName       = Environment.GetEnvironmentVariable("ApplicationName");
    
    if (!string.IsNullOrWhiteSpace(envIdentifier))
    {
        applicationInfo.ApplicationIdentifier = envIdentifier;
    }
    
    if (!string.IsNullOrWhiteSpace(envName))
    {
        applicationInfo.ApplicationName = envName;
    }

    builder.Services.AddSingleton(applicationInfo);

    var healthChecksBuilder = builder.Services.AddHealthChecks();
    
    var healthCheckConfig = builder.Configuration.GetSection("HealthChecks");
    var enablePostgres = healthCheckConfig.GetValue<bool>("EnablePostgreSQL");
    var enableRabbitMQ = healthCheckConfig.GetValue<bool>("EnableRabbitMQ");

    if (enablePostgres)
    {
        var postgresConnection = builder.Configuration.GetConnectionString("PostgreSQL");

        if (!string.IsNullOrWhiteSpace(postgresConnection))
        {
            healthChecksBuilder.AddNpgSql(
                postgresConnection,
                name: "postgres",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "postgres" });
            Log.Information("PostgreSQL health check enabled");
        }
        else
        {
            Log.Warning("PostgreSQL health check enabled but connection string is empty");
        }
    }

    if (enableRabbitMQ)
    {
        var rabbitMqConnection = builder.Configuration.GetConnectionString("RabbitMQ");

        if (!string.IsNullOrWhiteSpace(rabbitMqConnection))
        {
            healthChecksBuilder.AddRabbitMQ(
                sp => 
                {
                    var factory = new RabbitMQ.Client.ConnectionFactory();
                    factory.Uri = new Uri(rabbitMqConnection);

                    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
                },
                name: "rabbitmq",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "messaging", "rabbitmq" });
            Log.Information("RabbitMQ health check enabled");
        }
        else
        {
            Log.Warning("RabbitMQ health check enabled but connection string is empty");
        }
    }

    using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
    {
        loggingBuilder.AddSerilog();
    });
    var moduleLogger = loggerFactory.CreateLogger<ModuleLoader>();
    var moduleLoader = new ModuleLoader(builder.Configuration, moduleLogger);

    moduleLoader.LoadModules();

    moduleLoader.RegisterModuleServices(builder.Services);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        moduleLoader.UseModules(app);

        endpoints.MapGet("/", (ApplicationInfo appInfo) =>
        {
            return Results.Ok(appInfo);
        })
        .WithName("GetApplicationInfo")
        .WithTags("Application");

        endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description,
                        duration = e.Value.Duration.ToString(),
                        exception = e.Value.Exception?.Message,
                        data = e.Value.Data
                    }),
                    totalDuration = report.TotalDuration.ToString()
                });
                await context.Response.WriteAsync(result);
            }
        });
    });

    Log.Information("Application configured successfully");
    Log.Information("Loaded {ModuleCount} modules", moduleLoader.LoadedModules.Count);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
