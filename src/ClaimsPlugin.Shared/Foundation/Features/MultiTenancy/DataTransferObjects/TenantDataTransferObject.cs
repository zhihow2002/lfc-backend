using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

namespace Foundation.Features.MultiTenancy.DataTransferObjects;

public class TenantDataTransferObject : BaseDataTransferObject
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime ValidUpto { get; set; }
    public string? Issuer { get; set; }
}
