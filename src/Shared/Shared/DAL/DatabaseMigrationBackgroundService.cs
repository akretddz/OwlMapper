using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using static Shared.Consts.Configuration.Properties;
using static Shared.Consts.Configuration.Sections;

namespace Shared.DAL
{
    internal sealed class DatabaseMigrationBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILogger<DatabaseMigrationBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var shouldMigrate = configuration
                .GetSection(Database)
                .GetSection(Postgres)
                .GetValue<bool>(UseDatabaseMigrator);

            if (!shouldMigrate)
            {
                logger.LogInformation("Database migration is disabled. Skipping migration process.");

                return;
            }

            using var scope = serviceScopeFactory.CreateScope();
            var migrators = scope.ServiceProvider.GetServices<IDatabaseMigrator>();

            foreach (var migrator in migrators)
            {
                logger.LogInformation("Starting database migration using {MigratorType}.", migrator.GetType().Name);

                await migrator.MigrateAsync(stoppingToken);

                logger.LogInformation("Completed database migration using {MigratorType}.", migrator.GetType().Name);
            }
        }
    }
}
