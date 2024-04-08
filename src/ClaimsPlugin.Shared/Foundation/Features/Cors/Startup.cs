using ClaimsPlugin.Shared.Foundation.Features.Hosting.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Cors;

internal static class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    internal static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        CorsSettings? corsSettings = configuration
            .GetSection(nameof(CorsSettings))
            .Get<CorsSettings>();
        List<string> origins = new();

        if (corsSettings?.Origins is not null)
        {
            origins.AddRange(
                corsSettings.Origins.Split(';', StringSplitOptions.RemoveEmptyEntries)
            );
        }

        List<HostingSettings>? hostingSettings = configuration
            .GetSection("HostingSettings")
            .Get<List<HostingSettings>>();

        if (hostingSettings.IsNull())
        {
            throw new InvalidOperationException("Unable to read hosting setting.");
        }

        return services.AddCors(
            opt =>
                opt.AddPolicy(
                    CorsPolicy,
                    policy =>
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins(
                                origins
                                    .Concat(
                                        hostingSettings.Select(
                                            x => $"{x.RestHttps.Url}:{x.RestHttps.Port}"
                                        )
                                    )
                                    .Concat(
                                        hostingSettings.Select(
                                            x => $"{x.RestHttp.Url}:{x.RestHttp.Port}"
                                        )
                                    )
                                    .ToArray()
                            )
                )
        );
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        return app.UseCors(CorsPolicy);
    }
}
