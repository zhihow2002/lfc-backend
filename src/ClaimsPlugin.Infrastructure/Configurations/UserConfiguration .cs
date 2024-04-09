using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

         
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.Property(u => u.PasswordHash).IsRequired();

            // // Configure relationships
            // builder
            //     .HasMany(u => u.UserRoles)
            //     .WithOne() // Adjust based on your relationship configuration
            //     .HasForeignKey(ur => ur.UserId); // Adjust the foreign key name as necessary
        }
    }
}
