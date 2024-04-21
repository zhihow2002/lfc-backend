using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
internal class BaseDatabaseInitializer<TDbContext, TDbSeeder> : IDatabaseInitializer
    where TDbContext : DbContext, IDatabaseContext
    where TDbSeeder : class, IDatabaseSeeder
{
    private readonly TDbContext _dbContext;
    private readonly TDbSeeder _dbSeeder;
    private readonly ILogger<BaseDatabaseInitializer<TDbContext, TDbSeeder>> _logger;
    private readonly ICurrentTenant _currentTenant;
    private readonly DatabaseSettings _databaseSettings;
    public BaseDatabaseInitializer(
        TDbContext dbContext,
        ICurrentTenant currentTenant,
        TDbSeeder dbSeeder,
        ILogger<BaseDatabaseInitializer<TDbContext, TDbSeeder>> logger,
        IOptions<DatabaseSettings> databaseSettings
    )
    {
        _dbContext = dbContext;
        _currentTenant = currentTenant;
        _logger = logger;
        _databaseSettings = databaseSettings.Value;
        _dbSeeder = dbSeeder;
    }
    public async Task InitializeAsync(
        DatabaseInitializeMode mode,
        CancellationToken cancellationToken
    )
    {
        if (mode is DatabaseInitializeMode.OnlyMigration or DatabaseInitializeMode.Both)
        {
            _logger.LogInformation(
                "Getting Microservice Pending Migrations In Database {DatabaseName}",
                _dbContext.Database.GetDbConnection().Database
            );
            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    if (!_currentTenant.HasTenant())
                    {
                        _logger.LogInformation(
                            "Applying Microservice Migrations Without Tenant In Database {DatabaseName}",
                            _dbContext.Database.GetDbConnection().Database
                        );
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Applying Microservice Migrations for Tenant {TenantId} In Database {DatabaseName}",
                            _currentTenant.GetTenantId(),
                            _dbContext.Database.GetDbConnection().Database
                        );
                    }
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }
                else
                {
                    _logger.LogInformation("No Pending Microservice Migration Detected");
                }
            }
        }
        else
        {
            _logger.LogInformation(
                "Microservice Migrations In Database {DatabaseName} Is Skipped According To The Provided Mode",
                _dbContext.Database.GetDbConnection().Database
            );
        }
        if (mode is DatabaseInitializeMode.OnlyDataSeed or DatabaseInitializeMode.Both)
        {
            _logger.LogInformation(
                "Connecting To Database {DatabaseName} For Microservice",
                _dbContext.Database.GetDbConnection().Database
            );
            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation(
                    "Seeding Pre-configured Data In Database {DatabaseName}",
                    _dbContext.Database.GetDbConnection().Database
                );
                await _dbContext.StartSeedOperationAsync(
                    async () => await _dbSeeder.SeedDatabaseAsync(cancellationToken)
                );
            }
            else
            {
                _logger.LogError(
                    "Database {DatabaseName} Does Not Exist Or Could Not Be Connected",
                    _dbContext.Database.GetDbConnection().Database
                );
            }
        }
        else
        {
            _logger.LogInformation(
                "Microservice Seeding In Database {DatabaseName} Is Skipped According To The Provided Mode",
                _dbContext.Database.GetDbConnection().Database
            );
        }
    }
}
