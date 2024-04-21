using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Entities;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Models;
using Foundation.Features.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace Foundation.Features.MultiTenancy.Services;
internal class MultiTenancyDatabaseInitializer : IMultiTenancyDatabaseInitializer
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ILogger<MultiTenancyDatabaseInitializer> _logger;
    private readonly INotificationSender _notificationSender;
    private readonly TenantDatabaseContext _dbContext;
    public MultiTenancyDatabaseInitializer(
        TenantDatabaseContext tenantDatabaseContext,
        IOptions<DatabaseSettings> dbSettings,
        ILogger<MultiTenancyDatabaseInitializer> logger,
        INotificationSender notificationSender
    )
    {
        _dbContext = tenantDatabaseContext ?? throw new ArgumentNullException(nameof(tenantDatabaseContext));
        _dbSettings = dbSettings.Value ?? throw new ArgumentNullException(nameof(dbSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _notificationSender = notificationSender;
    }
    private async Task SeedDefaultTenantAsync(CancellationToken cancellationToken)
    {
        foreach (DefaultTenant defaultTenant in TenantConfiguration.GetAllDefaultTenantList())
        {
            if (await _dbContext.Tenants.FindAsync(new object?[] { defaultTenant.Id }, cancellationToken: cancellationToken) is null)
            {
                Tenant tenant = new(
                    defaultTenant.Id,
                    defaultTenant.Name,
                    _dbSettings.ConnectionDetail!
                );
                tenant.SetValidity(defaultTenant.ValidUpTo);
                _dbContext.Tenants.Add(tenant);
                await _dbContext.StartSeedOperationAsync(async () => await _dbContext.SaveChangesAsync(cancellationToken));
                await _notificationSender.BroadcastAsync(
                    new TenantNotification(defaultTenant.Id, defaultTenant.Name),
                    cancellationToken
                );
            }
        }
    }
    public async Task InitializeAsync(DatabaseInitializeMode mode, CancellationToken cancellationToken)
    {
        if (mode is DatabaseInitializeMode.OnlyMigration or DatabaseInitializeMode.Both)
        {
            _logger.LogInformation(
                "Getting Tenant Management Pending Migrations In Database {DatabaseName}",
                _dbContext.Database.GetDbConnection().Database
            );
            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation(
                        "Applying Tenant Management Migrations In Database {DatabaseName}",
                        _dbContext.Database.GetDbConnection().Database
                    );
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }
                else
                {
                    _logger.LogInformation("No Pending Tenant Management Migration Detected");
                }
            }
        }
        else
        {
            _logger.LogInformation("Tenant Migrations In Database {DatabaseName} Is Skipped According To The Provided Mode", _dbContext.Database.GetDbConnection().Database);
        }
        if (mode is DatabaseInitializeMode.OnlyDataSeed or DatabaseInitializeMode.Both)
        {
            _logger.LogInformation("Connecting To Database {DatabaseName} For Tenant Management", _dbContext.Database.GetDbConnection().Database);
            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Seeding Tenant Data In Database {DatabaseName} If Any", _dbContext.Database.GetDbConnection().Database);
                await SeedDefaultTenantAsync(cancellationToken);
            }
            else
            {
                _logger.LogError("Database {DatabaseName} Does Not Exist Or Could Not Be Connected", _dbContext.Database.GetDbConnection().Database);
            }
        }
        else
        {
            _logger.LogInformation("Tenant Seeding In Database {DatabaseName} Is Skipped According To The Provided Mode", _dbContext.Database.GetDbConnection().Database);
        }
    }
}
