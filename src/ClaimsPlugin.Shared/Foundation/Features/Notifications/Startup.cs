using ClaimsPlugin.Shared.Foundation.Features.Notifications.Hubs;
using ClaimsPlugin.Shared.Foundation.Features.Notifications.Models;
using ClaimsPlugin.Shared.Foundation.Features.Notifications.Providers;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ClaimsPlugin.Shared.Foundation.Features.Notifications;

internal static class Startup
{
    internal static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        Serilog.ILogger logger = Log.ForContext(typeof(Startup));

        SignalRSettings? signalRSettings = configuration.GetSection(nameof(SignalRSettings)).Get<SignalRSettings>();

        if (signalRSettings is
            {
                UseBackplane: false
            })
        {
            services.AddSignalR();
        }
        else
        {
            if (signalRSettings?.Backplane is null)
            {
                throw new InvalidOperationException("Backplane enabled, but no backplane settings in configuration.");
            }

            switch (signalRSettings.Backplane.Provider)
            {
                case "redis":
                    if (signalRSettings.Backplane.ConnectionString is null)
                    {
                        throw new InvalidOperationException("Redis backplane provider: No connectionString configured.");
                    }

                    services.AddSignalR()
                        .AddStackExchangeRedis(signalRSettings.Backplane.ConnectionString,
                            options =>
                            {
                                if (signalRSettings.Channel.IsNotNullOrWhiteSpace())
                                {
                                    options.Configuration.ChannelPrefix = signalRSettings.Channel;
                                }
                            } );
                    break;

                default:
                    throw new InvalidOperationException($"SignalR backplane Provider {signalRSettings.Backplane.Provider} is not supported.");
            }

            logger.Information("SignalR Backplane Current Provider: {BackplaneProvider}.", (signalRSettings.Backplane?.Provider ?? "-"));
        }

        return services
            .AddSingleton<IUserIdProvider, UserIdProvider>();
    }

    internal static IEndpointRouteBuilder MapNotifications(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<NotificationHub>(
            "/notifications",
            options =>
            {
                options.CloseOnAuthenticationExpiration = true;
                options.Transports = HttpTransportType.WebSockets;
            }
        );
        return endpoints;
    }
}
