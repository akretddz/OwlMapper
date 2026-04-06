using Account.Core.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DAL;

namespace Account.Core.Infrastructure
{
    internal static class Registration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgresDbContext<AccountDbContext>(configuration);
            services.AddTransient<IDatabaseMigrator, AccountDatabaseMigrator>();
        }
    }
}