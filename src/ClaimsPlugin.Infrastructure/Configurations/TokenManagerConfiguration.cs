using ClaimsPlugin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Infrastructure.Configurations
{
    public class TokenManagerConfiguration : IEntityTypeConfiguration<TokenManager>
    {
        public void Configure(EntityTypeBuilder<TokenManager> builder)
        {
            builder.ToTable("Tokens");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.AccessToken).IsRequired();
        }
    }
}
