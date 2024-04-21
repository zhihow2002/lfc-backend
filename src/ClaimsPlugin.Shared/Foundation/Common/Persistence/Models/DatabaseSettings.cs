using ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public class DatabaseSettings
{
    public string? ConnectionDetail { get; set; }
    public string? DatabaseName { get; set; }

    public bool IsMultiTenancy { get; set; }
    
    public string GetConnectionString(string? tenantId = null)
    {
        return DatabaseConnectionStringExtension.GetString(ConnectionDetail!, DatabaseName!, tenantId);
    }
}
