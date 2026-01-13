using HealthChecks.NpgSql;
using HealthChecks.RabbitMQ;
using OwlMapper.Shared;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
// Note: ReadFrom.Configuration causes the application to hang during startup.
// Using direct configuration for now. Seq sink can be added when Seq is available.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("Configuring services...");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure health checks
var healthChecksBuilder = builder.Services.AddHealthChecks();

// Add default health check
healthChecksBuilder.AddCheck("default", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Application is running"));

// Add PostgreSQL health check (will fail until configured properly in issue #4)
var postgresConnectionString = builder.Configuration["HealthChecks:Postgres:ConnectionString"];
if (!string.IsNullOrEmpty(postgresConnectionString))
{
    healthChecksBuilder.AddNpgSql(postgresConnectionString, name: "postgres");
}

// Add RabbitMQ health check (will fail until configured properly in issue #4)
var rabbitmqConnectionString = builder.Configuration["HealthChecks:RabbitMQ:ConnectionString"];
if (!string.IsNullOrEmpty(rabbitmqConnectionString))
{
    healthChecksBuilder.AddRabbitMQ(
        sp =>
        {
            var factory = new RabbitMQ.Client.ConnectionFactory();
            factory.Uri = new Uri(rabbitmqConnectionString);
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        },
        name: "rabbitmq"
    );
}

// Load modules
LoadModules(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseRouting();

// Map health checks endpoint
app.MapHealthChecks("/health");

// Map root endpoint to display APPLICATION_NAME and APPLICATION_IDENTIFIER
app.MapGet("/", () =>
{
    var applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "OwlMapper";
    var applicationIdentifier = Environment.GetEnvironmentVariable("APPLICATION_IDENTIFIER") ?? "bootstrapper";
    
    return Results.Json(new Dictionary<string, string>
    {
        { "APPLICATION_NAME", applicationName },
        { "APPLICATION_IDENTIFIER", applicationIdentifier }
    });
});

// Configure modules pipeline
ConfigureModulesPipeline(app);

try
{
    Log.Information("Starting OwlMapper Bootstrapper on http://localhost:2137");
    app.Run();
    Log.Information("Application stopped normally");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Module loading methods
static void LoadModules(IServiceCollection services, IConfiguration configuration)
{
    // Get all types that implement IModule
    var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

    foreach (var moduleType in moduleTypes)
    {
        var moduleName = moduleType.Name;
        var moduleConfigSection = configuration.GetSection($"Modules:{moduleName}");
        var moduleConfig = moduleConfigSection.Get<ModuleConfiguration>() ?? new ModuleConfiguration();

        if (moduleConfig.Enabled)
        {
            var module = (IModule)Activator.CreateInstance(moduleType)!;
            Log.Information("Loading module: {ModuleName}", module.Name);
            module.RegisterServices(services, configuration);
        }
        else
        {
            Log.Information("Module {ModuleName} is disabled in configuration", moduleName);
        }
    }
}

static void ConfigureModulesPipeline(WebApplication app)
{
    // Get all types that implement IModule
    var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

    foreach (var moduleType in moduleTypes)
    {
        var moduleName = moduleType.Name;
        var moduleConfigSection = app.Configuration.GetSection($"Modules:{moduleName}");
        var moduleConfig = moduleConfigSection.Get<ModuleConfiguration>() ?? new ModuleConfiguration();

        if (moduleConfig.Enabled)
        {
            var module = (IModule)Activator.CreateInstance(moduleType)!;
            Log.Information("Configuring module pipeline: {ModuleName}", module.Name);
            module.ConfigureApplication(app);
        }
    }
}
