namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;

public interface ICurrentTenantInitializer
{
    void SetCurrentTenantId(string tenantId);
}
