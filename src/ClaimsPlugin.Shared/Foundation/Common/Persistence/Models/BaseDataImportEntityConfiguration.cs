using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
public class BaseDataImportEntityConfiguration<TImportEntity, TImportRecordEntity> : IEntityTypeConfiguration<TImportEntity>
    where TImportEntity : BaseDataImportEntity<TImportRecordEntity>
    where TImportRecordEntity : BaseDataImportRecordEntity
{
    public virtual void Configure(EntityTypeBuilder<TImportEntity> builder)
    {
        builder.Property(x => x.CreatedBy).HasMaxLength(255);
        builder.Property(x => x.CreatedById).HasMaxLength(36);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(255);
        builder.Property(x => x.LastModifiedById).HasMaxLength(36);
        builder.Property(x => x.DeletedBy).HasMaxLength(255);
        builder.Property(x => x.DeletedById).HasMaxLength(36);
        builder.OwnsOne(x => x.Message, x => x.Property(xx => xx.Value).HasColumnName("Message").HasMaxLength(500));
        builder.OwnsOne(
            x => x.Remarks,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Remarks").HasMaxLength(1000);
            }
        );
        builder.OwnsOne(
            x => x.Status,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Status").HasMaxLength(50);
            }
        );
        builder.HasMany(x => x.Records)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
