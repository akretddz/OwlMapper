using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using static Shared.Consts.Configuration.Properties;
using static Shared.Consts.Configuration.Sections;

namespace Bootstrapper.HealthChecks
{
    public sealed class PostgresHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var connectionString = configuration
                .GetSection(Database)
                .GetSection(Postgres)
                .GetValue(ConnectionString, string.Empty);
            var dbCheckQuery = Consts.HealthChecks.PostgresDbCheckQuery;

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand(dbCheckQuery, connection);
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy(description: exception.Message, exception: exception);
            }
        }
    }
}