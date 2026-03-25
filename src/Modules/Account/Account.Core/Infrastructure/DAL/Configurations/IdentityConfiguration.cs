using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class IdentityConfiguration : IEntityTypeConfiguration<Identity>
    {
        public void Configure(EntityTypeBuilder<Identity> builder)
        {
            builder.ToTable(Identities);
            builder.HasKey(i => i.Id);

            builder.Property(i => i.IdentityProvider)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
