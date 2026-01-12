using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bootstrapper.HealthChecks
{
    public class DefaultHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                return await Task.FromResult(HealthCheckResult.Healthy());

            }
            catch (Exception ex)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}
