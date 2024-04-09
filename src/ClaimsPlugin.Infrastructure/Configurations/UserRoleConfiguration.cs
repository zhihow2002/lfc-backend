using ClaimsPlugin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Infrastructure.Configurations
{
    public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.RoleName).IsRequired().HasMaxLength(255);
        }
    }
}
