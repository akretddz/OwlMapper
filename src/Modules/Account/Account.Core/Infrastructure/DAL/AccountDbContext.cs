using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Account.Core.Infrastructure.DAL
{
    internal class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
    {
        internal virtual DbSet<Shared.Entities.Account> Accounts { get; init; } = null!;
        internal virtual DbSet<AccountIdentity> AccountIdentities { get; init; } = null!;
        internal virtual DbSet<AccountRole> AccountRoles { get; init; } = null!;
        internal virtual DbSet<AccountSecret> AccountSecrets { get; init; } = null!;
        internal virtual DbSet<AccountToken> AccountTokens { get; init; } = null!;
        internal virtual DbSet<Identity> Identities { get; init; } = null!;
        internal virtual DbSet<Role> Roles { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(Consts.Schema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}