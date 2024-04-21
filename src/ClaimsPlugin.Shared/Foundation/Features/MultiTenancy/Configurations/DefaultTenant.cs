namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;
internal class DefaultTenant
{
    public string Name { get; set; } = default!;
    public string Id { get; set; } = default!;
    public DateTime ValidUpTo { get; set; }
}
