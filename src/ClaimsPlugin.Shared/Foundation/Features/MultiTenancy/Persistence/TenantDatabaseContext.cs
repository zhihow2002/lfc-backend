//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Foundation.Features.MultiTenancy.Persistence;

//internal class TenantDatabaseContext : DbContext
//{
//    public bool IsSeedOperation { get; private set; }
    
//    public async Task StartSeedOperationAsync(Func<Task> action)
//    {
//        IsSeedOperation = true;

//        await action();
        
//        IsSeedOperation = false;
//    }
//    protected TenantDatabaseContext() { }
    
//    public DbSet<Tenant> Tenants => Set<Tenant>();
    
//    public TenantDatabaseContext(DbContextOptions<TenantDatabaseContext> options) : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        modelBuilder.ApplyConfiguration(new TenantConfiguration());
//    }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        // TODO: Maybe make this configurable in logger.json configuration because we only allow in development environment?
//        optionsBuilder.EnableSensitiveDataLogging();
//    }
//}
