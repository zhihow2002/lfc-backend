
using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;

public interface IAuthenticationService : ITransientService
{
    Task<IActionResult> AuthenticateAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken = default
    );
}
