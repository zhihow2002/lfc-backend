using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Helpers;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Persistence.Configurations;

public class AuditTrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("AuditTrails", SchemaNames.Auditing);

        builder.Property(x => x.AffectedColumns).HasColumnTypeAsNvarcharWithMaxLength();
        builder.Property(x => x.OldValues).HasColumnTypeAsNvarcharWithMaxLength();
        builder.Property(x => x.NewValues).HasColumnTypeAsNvarcharWithMaxLength();
    }
}
