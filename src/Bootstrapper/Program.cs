using OwlMapper.Bootstrapper.Models;
using OwlMapper.Bootstrapper.Modules;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting OwlMapper Bootstrapper");

    // Add services to the container
    builder.Services.AddOpenApi();

    // Configure ApplicationInfo from configuration and environment variables
    var applicationInfo = new ApplicationInfo();
    builder.Configuration.GetSection("ApplicationInfo").Bind(applicationInfo);
    
    // Override with environment variables if present
    var envIdentifier = Environment.GetEnvironmentVariable("ApplicationIdentifier");
    var envName = Environment.GetEnvironmentVariable("ApplicationName");
    
    if (!string.IsNullOrEmpty(envIdentifier))
    {
        applicationInfo.ApplicationIdentifier = envIdentifier;
    }
    
    if (!string.IsNullOrEmpty(envName))
    {
        applicationInfo.ApplicationName = envName;
    }

    builder.Services.AddSingleton(applicationInfo);

    // Configure Health Checks
    var healthChecksBuilder = builder.Services.AddHealthChecks();
    
    var healthCheckConfig = builder.Configuration.GetSection("HealthChecks");
    var enablePostgres = healthCheckConfig.GetValue<bool>("EnablePostgreSQL");
    var enableRabbitMQ = healthCheckConfig.GetValue<bool>("EnableRabbitMQ");

    if (enablePostgres)
    {
        var postgresConnection = builder.Configuration.GetConnectionString("PostgreSQL");
        if (!string.IsNullOrEmpty(postgresConnection))
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
        if (!string.IsNullOrEmpty(rabbitMqConnection))
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

    // Initialize Module Loader
    using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
    {
        loggingBuilder.AddSerilog();
    });
    var moduleLogger = loggerFactory.CreateLogger<ModuleLoader>();
    var moduleLoader = new ModuleLoader(builder.Configuration, moduleLogger);

    // Load modules
    moduleLoader.LoadModules();

    // Register module services
    moduleLoader.RegisterModuleServices(builder.Services);

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    // Use Serilog request logging
    app.UseSerilogRequestLogging();

    // Use routing
    app.UseRouting();

    // Configure endpoints
    app.UseEndpoints(endpoints =>
    {
        // Add modules to pipeline
        moduleLoader.UseModules(app);

        // Root endpoint - Application Info
        endpoints.MapGet("/", (ApplicationInfo appInfo) =>
        {
            return Results.Ok(appInfo);
        })
        .WithName("GetApplicationInfo")
        .WithTags("Application");

        // Health check endpoint
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

