using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Infrastructure.Configurations.EntityMap
{
    public class AuditLogMap : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.EventId);
            builder.Property(p => p.EventId).HasColumnType("bigint");
            builder.Property(p => p.JsonData).IsRequired();
            builder.Property(p => p.InsertedDate).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(p => p.EventType).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ReferenceId).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.IpAddress).HasMaxLength(50).IsRequired(false);
        }
    }
}