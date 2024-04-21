using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Microsoft.Extensions.Options;
namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Services;
public class CurrentTenant : ICurrentTenant, ICurrentTenantInitializer
{
    private readonly DatabaseSettings _dbSettings;
    private string? _tenantId;
    public CurrentTenant(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }
    public string? GetTenantId()
    {
        return _tenantId;
    }
    public string? GetConnectionString()
    {
        return !HasTenant() ? null : _dbSettings.GetConnectionString(_tenantId);
    }
    public bool HasTenant()
    {
        return _tenantId.IsNotNullOrWhiteSpace();
    }
    public void SetCurrentTenantId(string tenantId)
    {
        _tenantId = tenantId;
    }
}
