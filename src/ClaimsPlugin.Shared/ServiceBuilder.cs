using System.Reflection;
using ClaimsPlugin.Shared.Foundation.Features.HealthCheck;
using ClaimsPlugin.Shared.Foundation.Features.HttpClient;
using FluentValidation;

namespace ClaimsPlugin.Shared;

public static class ServiceBuilder
{
    public static IServiceCollection AddBaseApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly projectAssembly
    )
    {
        return services.AddValidatorsFromAssembly(projectAssembly).AddHttpClients(configuration);
    }

    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder builder,
        Assembly projectAssembly
    )
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();

        return builder;
    }
}
