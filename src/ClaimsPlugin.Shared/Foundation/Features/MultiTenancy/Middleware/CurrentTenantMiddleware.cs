using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;
using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using Microsoft.Extensions.Primitives;
namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Middleware;
public class CurrentTenantMiddleware : IMiddleware
{
    private readonly ICurrentTenantInitializer _currentTenantInitializer;
    public CurrentTenantMiddleware(ICurrentTenantInitializer currentTenantInitializer)
    {
        _currentTenantInitializer = currentTenantInitializer;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        bool header = context.Request.Headers.TryGetValue(TenantConfiguration.TenantIdFieldName, out StringValues tenantIdParam);
        if (header)
        {
            _currentTenantInitializer.SetCurrentTenantId(tenantIdParam.ToString());
        }
        else
        {
            bool queryString = context.Request.Query.TryGetValue(TenantConfiguration.TenantIdFieldName, out tenantIdParam);
            if (queryString)
            {
                _currentTenantInitializer.SetCurrentTenantId(tenantIdParam.ToString());
            }
        }
        await next(context);
    }
}
