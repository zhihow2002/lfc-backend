using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Entities;
public class Tenant : BaseEntity<string>
{
    protected Tenant()
    {
    }
    public Tenant(string id, string name, string? issuer = null)
    {
        Id = id;
        Name = name;
        IsActive = true;
        Issuer = issuer;
        ValidUpto = DateTime.Now.AddYears(10);
    }
    public bool IsActive { get; private set; }
    public DateTime ValidUpto { get; private set; }
    public string? Issuer { get; }
    public string Name { get; private set; } = default!;
    public void AddValidity(int months)
    {
        ValidUpto = ValidUpto.AddMonths(months);
    }
    public void SetValidity(in DateTime validTill)
    {
        ValidUpto = ValidUpto < validTill
            ? validTill
            : throw new Exception("Subscription cannot be backdated.");
    }
    public void Activate()
    {
        IsActive = true;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}
