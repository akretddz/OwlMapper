using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountSecretConfiguration : IEntityTypeConfiguration<AccountSecret>
    {
        public void Configure(EntityTypeBuilder<AccountSecret> builder)
        {
            builder.ToTable(AccountSecrets);
            builder.HasKey(@as => @as.Id);

            builder.Property(@as => @as.PasswordHash)
                .IsRequired();

            builder
                .HasOne(@as => @as.Account)
                .WithOne(a => a.AccountSecret)
                .HasForeignKey<AccountSecret>(@as => @as.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
