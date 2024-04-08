using ClaimsPlugin.Shared.Foundation.Features.HttpClient.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Features.HttpClient;

public static class Startup
{
    internal static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services.AddServices(typeof(IHttpClient));
    }

    private static IServiceCollection AddServices(
        this IServiceCollection services,
        Type interfaceType
    )
    {
        var interfaceTypes = AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .Select(
                t =>
                    new
                    {
                        Service = t.GetInterfaces()
                            .FirstOrDefault(
                                x => x.GetInterface(interfaceType.FullName!) is not null
                            ),
                        Implementation = t
                    }
            )
            .Where(t => t.Service is not null && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddSingleton(type.Service!, type.Implementation);
        }

        return services;
    }
}
