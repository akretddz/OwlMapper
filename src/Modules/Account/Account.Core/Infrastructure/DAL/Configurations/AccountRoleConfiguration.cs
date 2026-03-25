using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.ToTable(AccountRoles);

            builder.HasKey(ar => new
            {
                ar.AccountId,
                ar.RoleId
            });
        }
    }
}
