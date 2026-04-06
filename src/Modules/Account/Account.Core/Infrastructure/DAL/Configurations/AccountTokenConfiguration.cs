using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;
using static Account.Core.Infrastructure.Consts.Validations;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class AccountTokenConfiguration : IEntityTypeConfiguration<AccountToken>
    {
        public void Configure(EntityTypeBuilder<AccountToken> builder)
        {
            builder.ToTable(AccountTokens);
            builder.HasKey(at => at.Id);

            builder.Property(at => at.Token)
                .IsRequired()
                .HasMaxLength(TokenMaxLength);

            builder.Property(at => at.TokenType)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(TokenTypeMaxLength);

            builder
                .HasOne(at => at.Account)
                .WithMany(a => a.AccountTokens)
                .HasForeignKey(at => at.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}