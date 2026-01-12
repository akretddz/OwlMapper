using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Bootstrapper.HealthChecks
{
    public sealed class RabbitMQHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connectionString = configuration
                .GetSection("MessageBroker")
                .GetSection("RabbitMQ")
                .GetValue("ConnectionString", string.Empty);

            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(connectionString)
                };

                await using var connection = await factory.CreateConnectionAsync(cancellationToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Degraded(description: exception.Message, exception: exception);
            }
        }
    }
}
