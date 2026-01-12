using Bootstrapper;
using Bootstrapper.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog();
builder.Services
    .AddHealthChecks()
    .AddCheck<DefaultHealthCheck>(Consts.HealthChecks.Default)
    .AddCheck<PostgresHealthCheck>(Consts.HealthChecks.Postgres)
    .AddCheck<RabbitMQHealthCheck>(Consts.HealthChecks.RabbitMQ);


var app = builder.Build();

// app.MapGet(ze zmiennym śr.);

app.MapHealthChecks("health");

await app.RunAsync();