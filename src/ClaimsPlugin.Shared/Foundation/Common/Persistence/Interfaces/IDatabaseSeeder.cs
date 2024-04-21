namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

public interface IDatabaseSeeder
{
    public Task SeedDatabaseAsync(CancellationToken cancellationToken);
}
