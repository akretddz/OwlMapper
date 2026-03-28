using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static Shared.Consts.Configuration.Properties;
using static Shared.Consts.Configuration.Sections;

namespace Shared.DAL
{
    public static class Registration
    {
        public static IServiceCollection AddPostgresDbContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var connectionString = configuration
                .GetSection(Database)
                .GetSection(Postgres)
                .GetValue(ConnectionString, string.Empty);

            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);

                        npgsqlOptions.CommandTimeout(60);
                    }));

            return services;
        }

        internal static IServiceCollection AddDatabaseMigrator(
            this IServiceCollection services)
        {
            services.AddHostedService<DatabaseMigrationBackgroundService>();

            return services;
        }
    }
}
