using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;
using static Account.Core.Infrastructure.Consts.Validations;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountSecretConfiguration : IEntityTypeConfiguration<AccountSecret>
    {
        public void Configure(EntityTypeBuilder<AccountSecret> builder)
        {
            builder.ToTable(AccountSecrets);
            builder.HasKey(@as => @as.Id);

            builder.Property(@as => @as.Type)
                .HasConversion<string>()
                .HasMaxLength(SecretTypeMaxLength);

            builder.Property(@as => @as.Value)
                .IsRequired()
                .HasMaxLength(SecretValueMaxLength);

            builder
                .HasOne(@as => @as.Account)
                .WithMany(a => a.AccountSecrets)
                .HasForeignKey(@as => @as.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}