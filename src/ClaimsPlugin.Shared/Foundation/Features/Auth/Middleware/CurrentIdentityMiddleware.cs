using ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Middleware;

public class CurrentIdentityMiddleware : IMiddleware
{
    private readonly ICurrentIdentityInitializer _currentIdentityInitializer;

    public CurrentIdentityMiddleware(ICurrentIdentityInitializer currentIdentityInitializer)
    {
        _currentIdentityInitializer = currentIdentityInitializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentIdentityInitializer.SetCurrentIdentity(context.User);

        await next(context);
    }
}
