namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;

internal static class TenantConfiguration
{
    /// <summary>
    ///     Used to configure the name for identifier
    /// </summary>
    internal const string TenantIdFieldName = "Tenant";

    internal static IEnumerable<DefaultTenant> GetAllDefaultTenantList()
    {
        yield return new DefaultTenant { Name = "GE", Id = "GE", ValidUpTo = DateTime.Now.AddYears(99) };
    }
}
