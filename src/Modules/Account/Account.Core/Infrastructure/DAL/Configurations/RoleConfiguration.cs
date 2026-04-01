using Account.Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Account.Core.Infrastructure.Consts.Tables.Names;

namespace Account.Core.Infrastructure.DAL.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(Roles);
            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.RoleName)
                .IsUnique();
            builder.Property(r => r.RoleName)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}