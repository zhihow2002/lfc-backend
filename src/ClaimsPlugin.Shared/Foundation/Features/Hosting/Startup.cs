using ClaimsPlugin.Shared.Foundation.Features.Hosting.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace ClaimsPlugin.Shared.Foundation.Features.Hosting;

public static class Startup
{
    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(Startup));

    public static void ConfigureEndpoints(
        this KestrelServerOptions options,
        string microservice,
        IWebHostEnvironment environment,
        IConfiguration configuration
    )
    {
        // Use direct null check
        var hostingSettings = configuration
            .GetSection("HostingSettings")
            .Get<List<HostingSettings>>();

        if (hostingSettings == null)
        {
            _logger.Error(
                "Unable to read hosting settings. The 'HostingSettings' section is missing or empty."
            );
            throw new InvalidOperationException("Unable to read hosting setting.");
        }

        // Use direct null and any check
        var setting = hostingSettings.FirstOrDefault(
            x => string.Equals(x.Name, microservice, StringComparison.OrdinalIgnoreCase)
        );

        if (setting == null)
        {
            _logger.Error(
                "Unable to find hosting setting for microservice: {Microservice}",
                microservice
            );
            throw new InvalidOperationException("Unable to read particular hosting setting.");
        }

        if (environment.IsEnvironment("Development"))
        {
            ConfigureDevelopmentEndpoints(options, setting);
        }
        else
        {
            ConfigureProductionEndpoints(options, setting);
        }
    }

    private static void ConfigureDevelopmentEndpoints(
        KestrelServerOptions options,
        HostingSettings setting
    )
    {
        // Check for null on RestHttps, RestHttp, and Grpc before accessing
        if (setting.RestHttps?.Enabled == true)
        {
            options.ListenLocalhost(
                setting.RestHttps.Port,
                listenOptions =>
                {
                    listenOptions.UseHttps();
                    listenOptions.Protocols = HttpProtocols.Http1;
                }
            );
        }

        if (setting.RestHttp?.Enabled == true)
        {
            options.ListenLocalhost(
                setting.RestHttp.Port,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                }
            );
        }

        if (setting.Grpc?.Enabled == true)
        {
            options.ListenLocalhost(
                setting.Grpc.Port,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                }
            );
        }
    }

    private static void ConfigureProductionEndpoints(
        KestrelServerOptions options,
        HostingSettings setting
    )
    {
        // Similar null checks as in ConfigureDevelopmentEndpoints
        if (setting.RestHttps?.Enabled == true)
        {
            options.ListenAnyIP(
                setting.RestHttps.Port,
                listenOptions =>
                {
                    listenOptions.UseHttps();
                    listenOptions.Protocols = HttpProtocols.Http1;
                }
            );
        }

        if (setting.RestHttp?.Enabled == true)
        {
            options.ListenAnyIP(
                setting.RestHttp.Port,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                }
            );
        }

        if (setting.Grpc?.Enabled == true)
        {
            options.ListenAnyIP(
                setting.Grpc.Port,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                }
            );
        }
    }

    public static IServiceCollection AddForwardedHeaders(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    public static IApplicationBuilder UseForwardedHeaders(
        this IApplicationBuilder app,
        IConfiguration configuration
    )
    {
        app.UseForwardedHeaders();
        return app;
    }
}
