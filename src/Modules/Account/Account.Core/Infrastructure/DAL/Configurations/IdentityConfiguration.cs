using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

using IdentityEntityType = Account.Core.Shared.Entities.Identity;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class IdentityConfiguration : IEntityTypeConfiguration<IdentityEntityType>
    {
        public void Configure(EntityTypeBuilder<IdentityEntityType> builder)
        {
            builder.ToTable(Identities);
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Provider)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}