using Bootstrapper.HealthChecks;
using Shared;
using Shared.Exceptions;
using Shared.Modules;

using static Bootstrapper.Consts;
using static Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureModules();

var assembliesList = ModuleLoader.LoadAssemblies(builder.Configuration);
var modulesList    = ModuleLoader.LoadModules(assembliesList);

modulesList.ForEach(module => module.Register(builder.Services, builder.Configuration));

builder.Services.AddShared();

builder.Services
    .AddHealthChecks()
    .AddCheck<PostgresHealthCheck>(HealthChecks.Postgres)
    .AddCheck<RabbitMQHealthCheck>(HealthChecks.RabbitMQ);

var app = builder.Build();

app.UseExceptionMiddleware();

modulesList.ForEach(module => module.Use(app));

app.MapHealthChecks("/health");

app.MapGet("/",
    context => context.Response.WriteAsJsonAsync(new
    {
        ApplicationInfo.Name,
        ApplicationInfo.ApplicationCode,
    }));

#if DEBUG
app.MapGet("/test/test-exception", () =>
{
    throw new TestException();
});

app.MapGet("/test/validation-exception", () =>
{
    var errors = new Dictionary<string, string[]>
    {
        { "email",    ["Email is required.", "Email format is invalid."] },
        { "password", ["Password must be at least 8 characters."] },
    };
    throw new ValidationException(errors);
});

app.MapGet("/test/internal-exception", () =>
{
    throw new InvalidOperationException("Sensitive internal crash info.");
});
#endif

await app.RunAsync();

#if DEBUG
file sealed class TestAppException(string errorCode, string message, int statusCode)
    : AppException(errorCode, message, statusCode);
#endif