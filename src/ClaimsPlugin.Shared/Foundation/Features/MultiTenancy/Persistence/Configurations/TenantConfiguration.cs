//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;

//public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
//{
//    public void Configure(EntityTypeBuilder<Tenant> builder)
//    {
//        builder
//            .ToTable("Tenants", SchemaNames.MultiTenancy);

//        builder.Property(x => x.Name).HasMaxLength(100);
//        builder.Property(x => x.Id).HasMaxLength(50);
//    }
//}
