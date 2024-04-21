

using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Attributes;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;

namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Attributes;

public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantIdHeaderAttribute()
        : base(
            TenantConfiguration.TenantIdFieldName,
            "Input your tenant Id to access this API",
            string.Empty,
            true
        )
    {
    }
}
