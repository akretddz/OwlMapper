using Bootstrapper.HealthChecks;
using Shared.Modules;

using static Bootstrapper.Consts;
using static Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

var assembliesList = ModuleLoader.LoadAssemblies(builder.Configuration);
var modulesList    = ModuleLoader.LoadModules(assembliesList);

modulesList.ForEach(module => module.Register(builder.Services, builder.Configuration));

builder.Services
    .AddHealthChecks()
    .AddCheck<DefaultHealthCheck>(HealthChecks.Default)
    .AddCheck<PostgresHealthCheck>(HealthChecks.Postgres)
    .AddCheck<RabbitMQHealthCheck>(HealthChecks.RabbitMQ);


var app = builder.Build();

app.MapGet("/",
    context => context.Response.WriteAsJsonAsync(new
    {
        ApplicationInfo.Name,
        ApplicationInfo.ApplicationCode,
    }));

app.MapHealthChecks("health");

await app.RunAsync();