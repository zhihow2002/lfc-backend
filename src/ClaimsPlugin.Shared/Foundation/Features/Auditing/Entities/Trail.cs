using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;

public class Trail : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string? Type { get; set; } = default!;
    public string? TableName { get; set; } = default!;
    public DateTime DateTime { get; set; } = default!;
    public string? OldValues { get; set; } = default!;
    public string? NewValues { get; set; } = default!;
    public string? AffectedColumns { get; set; } = default!;
    public string? PrimaryKey { get; set; } = default!;
}
