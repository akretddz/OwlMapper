using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DAL;

namespace Shared
{
    public static class Registration
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseMigrator();

            return services;
        }
    }
}
