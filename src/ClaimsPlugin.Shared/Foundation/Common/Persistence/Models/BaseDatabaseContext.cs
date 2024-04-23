//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;
//using ClaimsPlugin.Shared.Foundation.Features.Auditing.Enums;
//using ClaimsPlugin.Shared.Foundation.Features.Auditing.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.Auditing.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Auditing.Persistence.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.EventSourcing.Sources.Domain.Models;
//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.Extensions.Options;
//namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
//public abstract class BaseDatabaseContext : DbContext,
//    IAuditableDbContext,
//    IDatabaseContext
//{
//    public bool IsSeedOperation { get; private set; }
//    public async Task StartSeedOperationAsync(Func<Task> action)
//    {
//        IsSeedOperation = true;
//        await action();
//        IsSeedOperation = false;
//    }
//    private readonly ICurrentIdentity _currentIdentity;
//    private readonly ICurrentTenant _currentTenant;
//    private readonly DatabaseSettings _dbSettings;
//    private readonly IDomainEventPublisher _domainEventPublisher;
//    private readonly ISerializerService _serializer;
//    public BaseDatabaseContext(
//        DbContextOptions options,
//        ICurrentIdentity currentIdentity,
//        ICurrentTenant currentTenant,
//        ISerializerService serializer,
//        IOptions<DatabaseSettings> dbSettings,
//        IDomainEventPublisher domainEventPublisher
//    ) : base(options)
//    {
//        _currentIdentity = currentIdentity;
//        _currentTenant = currentTenant;
//        _serializer = serializer;
//        _dbSettings = dbSettings.Value;
//        _domainEventPublisher = domainEventPublisher;
//        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
//    }
//    public DbSet<Trail> AuditTrails => Set<Trail>();
//    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
//    {
//        List<AuditTrail> auditEntries = HandleAuditingBeforeSaveChanges();
//        int result = await base.SaveChangesAsync(cancellationToken);
//        await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);
//        await SendDomainEventsAsync();
//        return result;
//    }
//    internal List<AuditTrail> HandleAuditingBeforeSaveChanges()
//    {
//        string identityType;
//        Guid identityId;
//        string identityName;
//        if (IsSeedOperation)
//        {
//            identityType = "System";
//            identityId = Guid.Empty;
//            identityName = "System";
//        }
//        else
//        {
//            identityType = _currentIdentity.GetIdentityType().ToString();
//            identityId = _currentIdentity.GetIdentityId();
//            identityName = _currentIdentity.GetIdentityName();
//        }
//        foreach (EntityEntry<IAuditableEntity> entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
//        {
//            switch (entry.State)
//            {
//                case EntityState.Added:
//                    entry.Entity.CreatedBy = identityName;
//                    entry.Entity.CreatedById = identityId;
//                    entry.Entity.CreatedByType = identityType;
//                    entry.Entity.LastModifiedBy = identityName;
//                    entry.Entity.LastModifiedById = identityId;
//                    entry.Entity.LastModifiedByType = identityType;
//                    break;
//                case EntityState.Modified:
//                    entry.Entity.LastModifiedOn = DateTime.Now;
//                    entry.Entity.LastModifiedBy = identityName;
//                    entry.Entity.LastModifiedById = identityId;
//                    entry.Entity.LastModifiedByType = identityType;
//                    break;
//                case EntityState.Deleted:
//                    SoftDelete(entry, identityType, identityId, identityName);
//                    foreach (NavigationEntry navigationEntry in entry.Navigations.Where(x => !((IReadOnlyNavigation)x.Metadata).IsOnDependent))
//                    {
//                        if (navigationEntry is CollectionEntry collectionEntry)
//                        {
//                            if (collectionEntry.CurrentValue != null)
//                            {
//                                collectionEntry.Load();
//                                foreach (object? dependentEntry in collectionEntry.CurrentValue)
//                                {
//                                    SoftDelete(Entry(dependentEntry), identityType, identityId, identityName);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            navigationEntry.Load();
//                            object? dependentEntry = navigationEntry.CurrentValue;
//                            if (dependentEntry != null)
//                            {
//                                SoftDelete(Entry(dependentEntry), identityType, identityId, identityName);
//                            }
//                        }
//                    }
//                    break;
//            }
//        }
//        ChangeTracker.DetectChanges();
//        List<AuditTrail> trailEntries = new();
//        foreach (EntityEntry<IAuditableEntity> entry in ChangeTracker.Entries<IAuditableEntity>()
//                     .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
//                     .ToList())
//        {
//            AuditTrail trailEntry = new(entry, _serializer) { TableName = entry.Entity.GetType().Name, UserId = identityId.ToString() };
//            trailEntries.Add(trailEntry);
//            foreach (PropertyEntry property in entry.Properties)
//            {
//                if (property.IsTemporary)
//                {
//                    trailEntry.TemporaryProperties.Add(property);
//                    continue;
//                }
//                string propertyName = property.Metadata.Name;
//                if (property.Metadata.IsPrimaryKey())
//                {
//                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
//                    continue;
//                }
//                switch (entry.State)
//                {
//                    case EntityState.Added:
//                        trailEntry.TrailType = TrailType.Create;
//                        trailEntry.NewValues[propertyName] = property.CurrentValue;
//                        break;
//                    case EntityState.Deleted:
//                        trailEntry.TrailType = TrailType.Delete;
//                        trailEntry.OldValues[propertyName] = property.OriginalValue;
//                        break;
//                    case EntityState.Modified:
//                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
//                        {
//                            trailEntry.ChangedColumns.Add(propertyName);
//                            trailEntry.TrailType = TrailType.Delete;
//                            trailEntry.OldValues[propertyName] = property.OriginalValue;
//                            trailEntry.NewValues[propertyName] = property.CurrentValue;
//                        }
//                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
//                        {
//                            trailEntry.ChangedColumns.Add(propertyName);
//                            trailEntry.TrailType = TrailType.Update;
//                            trailEntry.OldValues[propertyName] = property.OriginalValue;
//                            trailEntry.NewValues[propertyName] = property.CurrentValue;
//                        }
//                        break;
//                }
//            }
//        }
//        foreach (AuditTrail auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
//        {
//            AuditTrails.Add(auditEntry.ToAuditTrail());
//        }
//        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
//    }
//    internal Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, in CancellationToken cancellationToken = new())
//    {
//        if (trailEntries is null || trailEntries.Count == 0)
//        {
//            return Task.CompletedTask;
//        }
//        foreach (AuditTrail entry in trailEntries)
//        {
//            foreach (PropertyEntry prop in entry.TemporaryProperties)
//            {
//                if (prop.Metadata.IsPrimaryKey())
//                {
//                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
//                }
//                else
//                {
//                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
//                }
//            }
//            AuditTrails.Add(entry.ToAuditTrail());
//        }
//        return SaveChangesAsync(cancellationToken);
//    }
//    internal async Task SendDomainEventsAsync()
//    {
//        IEntity[] entitiesWithEvents = ChangeTracker.Entries<IEntity>()
//            .Select(e => e.Entity)
//            .Where(e => e.DomainEvents.Count > 0)
//            .ToArray();
//        foreach (IEntity entity in entitiesWithEvents)
//        {
//            DomainEvent[] domainEvents = entity.DomainEvents.ToArray();
//            entity.DomainEvents.Clear();
//            foreach (DomainEvent domainEvent in domainEvents)
//            {
//                await _domainEventPublisher.PublishAsync(domainEvent);
//            }
//        }
//    }
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
//        modelBuilder.ApplyConfiguration(new AuditTrailConfiguration());
//        base.OnModelCreating(modelBuilder);
//    }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        if (_dbSettings.IsMultiTenancy)
//        {
//            if (!_currentTenant.HasTenant())
//            {
//                optionsBuilder.UseDatabase(_dbSettings.GetConnectionString());
//            }
//            else
//            {
//                optionsBuilder.UseDatabase(_currentTenant.GetConnectionString());
//            }
//        }
//        else
//        {
//            optionsBuilder.UseDatabase(_dbSettings.GetConnectionString());
//        }
//    }
//    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
//    {
//        configurationBuilder.Properties<string>().AreUnicode().HaveMaxLength(300);
//        configurationBuilder.Properties<int>().HaveMaxLength(20);
//        configurationBuilder.Properties<long>().HaveMaxLength(4000);
//        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
//    }
//    private void SoftDelete(EntityEntry entry, string type, Guid id, string name)
//    {
//        if (entry.Entity is ISoftDelete softDelete)
//        {
//            softDelete.DeletedBy = name;
//            softDelete.DeletedOn = DateTime.Now;
//            softDelete.DeletedById = id;
//            softDelete.DeletedByType = type;
//            entry.State = EntityState.Modified;
//        }
//    }
//}
