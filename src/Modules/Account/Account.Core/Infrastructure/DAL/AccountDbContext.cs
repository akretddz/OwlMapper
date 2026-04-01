using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Account.Core.Infrastructure.DAL
{
    internal class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {

        }

        internal virtual DbSet<Shared.Entities.Account> Accounts { get; init; }
        internal virtual DbSet<AccountIdentity> AccountIdentities { get; init; }
        internal virtual DbSet<AccountRole> AccountRoles { get; init; }
        internal virtual DbSet<AccountSecret> AccountSecrets { get; init; }
        internal virtual DbSet<AccountToken> AccountTokens { get; init; }
        internal virtual DbSet<Identity> Identities { get; init; }
        internal virtual DbSet<Role> Roles { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(Consts.Schema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}