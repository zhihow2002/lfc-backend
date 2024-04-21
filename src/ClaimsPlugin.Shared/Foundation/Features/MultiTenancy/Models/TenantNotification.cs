namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Models;

public class TenantNotification : INotificationMessage
{
    public string TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Issuer { get; private set; }
    public string MethodName { get; private set; }

    public TenantNotification(string tenantId, string name, string? issuer = null)
    {
        TenantId = tenantId;
        Name = name;
        Issuer = issuer;
        MethodName = nameof(TenantNotification);
    }
}
