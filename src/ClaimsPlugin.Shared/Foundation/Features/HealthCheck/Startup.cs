using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ClaimsPlugin.Shared.Foundation.Features.HealthCheck;

public static class Startup
{
    internal static IServiceCollection AddHealthCheck(this IServiceCollection services)
    {
        return services
            .AddGrpcHealthChecks()
            .Services.AddHealthChecks()
            .AddCheck("Self", () => HealthCheckResult.Healthy())
            .Services;
    }

    internal static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks("/api/health");
    }
}
