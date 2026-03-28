using Microsoft.Extensions.DependencyInjection;
using Shared.DAL;

namespace Shared
{
    public static class Registration
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddDatabaseMigrator();

            return services;
        }
    }
}
