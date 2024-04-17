using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseAuditableEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.CreatedBy).HasMaxLength(255);
        builder.Property(x => x.CreatedById).HasMaxLength(36);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(255);
        builder.Property(x => x.LastModifiedById).HasMaxLength(36);
        builder.Property(x => x.DeletedBy).HasMaxLength(255);
        builder.Property(x => x.DeletedById).HasMaxLength(36);
    }
}
