using ClaimsPlugin.Shared.Foundation.Features.HttpClient.Handlers;
using RestSharp;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ClaimsPlugin.Shared.Foundation.HttpClient.Models
{
    public class BaseHttpClient : IDisposable
    {
        protected readonly RestClient Client;
        protected readonly ILogger Logger;

        protected BaseHttpClient(string baseUrl, int maxTimeout = 6000)
        {
            LoggerFactory loggerFactory = new();

            loggerFactory.AddSerilog(Log.ForContext<BaseHttpClient>());

            Logger = loggerFactory.CreateLogger<BaseHttpClient>();

            RestClientOptions options =
                new(baseUrl)
                {
                    MaxTimeout = maxTimeout,
                    UserAgent = "LFC-Claims-Plugin",
                    ConfigureMessageHandler = handler => new LogDelegatingHandler(Logger, handler)
                };

            Client = new RestClient(options);
        }

        protected BaseHttpClient(int maxTimeout = 6000)
        {
            LoggerFactory loggerFactory = new();

            loggerFactory.AddSerilog(Log.ForContext<BaseHttpClient>());

            Logger = loggerFactory.CreateLogger<BaseHttpClient>();

            RestClientOptions options =
                new()
                {
                    MaxTimeout = maxTimeout,
                    UserAgent = "LFC-Claims-Plugin",
                    ConfigureMessageHandler = handler => new LogDelegatingHandler(Logger, handler)
                };

            Client = new RestClient(options);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
