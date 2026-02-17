using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using static Shared.Consts.Configuration.Properties;
using static Shared.Consts.Configuration.Sections;

namespace Bootstrapper.HealthChecks
{
    public sealed class RabbitMQHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var rabbitMqSection = configuration
                .GetSection(Messaging)
                .GetSection(Rabbit);

            try
            {
                var factory = new ConnectionFactory
                {
                    Port     = rabbitMqSection.GetValue("port", 5672),
                    UserName = rabbitMqSection.GetValue<string>("username", "guest"),
                    Password = rabbitMqSection.GetValue<string>("password", "guest")
                };

                var hostNames = rabbitMqSection
                    .GetSection("hostNames")
                    .Get<IEnumerable<string>>() ?? ["localhost"];

                await using var connection = await factory.CreateConnectionAsync(hostNames, cancellationToken);
                await using var channel    = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Degraded(description: exception.Message, exception: exception);
            }
        }
    }
}