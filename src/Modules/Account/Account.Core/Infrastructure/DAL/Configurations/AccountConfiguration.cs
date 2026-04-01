using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;
using static Account.Core.Infrastructure.Consts.Validations;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Shared.Entities.Account>
    {
        public void Configure(EntityTypeBuilder<Shared.Entities.Account> builder)
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