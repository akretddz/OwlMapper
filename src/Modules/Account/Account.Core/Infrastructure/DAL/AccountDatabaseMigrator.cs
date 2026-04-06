using Microsoft.EntityFrameworkCore;
using Shared.DAL;

namespace Account.Core.Infrastructure.DAL
{
    internal class AccountDatabaseMigrator(AccountDbContext dbContext) : IDatabaseMigrator
    {
        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}