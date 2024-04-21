using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public class BaseDataImportRecordEntityConfiguration<TImportRecordEntity> : IEntityTypeConfiguration<TImportRecordEntity>
    where TImportRecordEntity : BaseDataImportRecordEntity
{
    public virtual void Configure(EntityTypeBuilder<TImportRecordEntity> builder)
    {
        builder.OwnsOne(
            x => x.Message,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Message").HasColumnOrder(3).HasMaxLength(500);
            }
        );

        builder.OwnsOne(
            x => x.Remarks,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Remarks").HasColumnOrder(4).HasMaxLength(1000);
            }
        );

        builder.OwnsOne(
            x => x.Status,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Status").HasColumnOrder(2).HasMaxLength(50);
            }
        );

        builder.OwnsOne(
            x => x.Action,
            x =>
            {
                x.Property(xx => xx.Value).HasColumnName("Action").HasColumnOrder(1).HasMaxLength(50);
            }
        );

        builder.Property(x => x.ProcessedOn).HasColumnOrder(5);
    }
}
