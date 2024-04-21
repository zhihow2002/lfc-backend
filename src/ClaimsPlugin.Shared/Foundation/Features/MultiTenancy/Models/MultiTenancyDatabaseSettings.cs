using ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;

namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Models;
public class MultiTenancyDatabaseSettings
{
    public string ConnectionDetail { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;

    public string GetConnectionString()
    {
        return DatabaseConnectionStringExtension.GetString(ConnectionDetail, DatabaseName);
    }
}
