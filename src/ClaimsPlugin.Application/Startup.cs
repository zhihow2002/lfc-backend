using ClaimsPlugin.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ClaimsPlugin.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services.AddBaseApplication(configuration, typeof(Startup).Assembly);
    }
}
