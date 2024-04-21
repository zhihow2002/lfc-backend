namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;

public interface ICurrentTenant
{
    string? GetTenantId();

    string? GetConnectionString();
    
    bool HasTenant();
}
