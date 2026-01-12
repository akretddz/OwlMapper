using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Bootstrapper.HealthChecks
{
    public sealed class PostgresHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var connectionString = configuration
                .GetSection("Database")
                .GetSection("Postgres")
                .GetValue("ConnectionString", string.Empty);
            var dbCheckQuery = "SELECT 1";

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand(dbCheckQuery, connection);
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
