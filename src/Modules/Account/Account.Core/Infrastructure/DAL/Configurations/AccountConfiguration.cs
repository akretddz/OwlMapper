using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;
using static Account.Core.Infrastructure.Consts.Validations;

using AccountEntityType = Account.Core.Shared.Entities.Account;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<AccountEntityType>
    {
        public void Configure(EntityTypeBuilder<AccountEntityType> builder)
        {
            builder.ToTable(Accounts);
            builder.HasKey(a => a.Id);

            builder.Property(a => a.EmailAddress)
                .IsRequired()
                .HasMaxLength(EmailAddressMaxLength);
            builder
                .HasIndex(a => a.EmailAddress)
                .IsUnique();

            builder.Property(a => a.Username)
                .HasMaxLength(UsernameMaxLength);

            builder
                .HasMany(a => a.AccountRoles)
                .WithOne(ar => ar.Account)
                .HasForeignKey(ar => ar.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(a => a.AccountTokens)
                .WithOne(at => at.Account)
                .HasForeignKey(at => at.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}