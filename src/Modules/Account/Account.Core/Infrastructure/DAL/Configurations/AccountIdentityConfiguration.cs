using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountIdentityConfiguration : IEntityTypeConfiguration<AccountIdentity>
    {
        public void Configure(EntityTypeBuilder<AccountIdentity> builder)
        {
            builder.ToTable(AccountIdentities);
            builder.HasKey(ai => new
            {
                ai.AccountId,
                ai.IdentityId
            });
        }
    }
}