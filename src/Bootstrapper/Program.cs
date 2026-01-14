using Bootstrapper.HealthChecks;
using Shared.Modules;

using static Bootstrapper.Consts;
using static Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureModules();

var assembliesList = ModuleLoader.LoadAssemblies(builder.Configuration);
var modulesList    = ModuleLoader.LoadModules(assembliesList);

modulesList.ForEach(module => module.Register(builder.Services, builder.Configuration));

builder.Services
    .AddHealthChecks()
    .AddCheck<PostgresHealthCheck>(HealthChecks.Postgres)
    .AddCheck<RabbitMQHealthCheck>(HealthChecks.RabbitMQ);

var app = builder.Build();

app.MapGet("/",
    context => context.Response.WriteAsJsonAsync(new
    {
        ApplicationInfo.Name,
        ApplicationInfo.ApplicationCode,
    }));

app.MapHealthChecks("/health");

await app.RunAsync();