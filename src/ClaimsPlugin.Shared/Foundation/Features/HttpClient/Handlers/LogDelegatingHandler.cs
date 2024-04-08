using System.Diagnostics;

namespace ClaimsPlugin.Shared.Foundation.Features.HttpClient.Handlers;

public class LogDelegatingHandler : DelegatingHandler
{
    private readonly ILogger _logger;

    public LogDelegatingHandler(ILogger logger, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "HTTP {Method} Request starting to '{Request}'",
            request.Method,
            request.RequestUri
        );

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation(
                "HTTP {Method} Request sent to '{Request}' and done in {Duration}",
                request.Method.Method,
                request.RequestUri,
                stopwatch.Elapsed
            );
        }
        else
        {
            _logger.LogError(
                "HTTP {Method} Request sent to '{Request}' and done in {Duration} with errors",
                request.Method.Method,
                request.RequestUri,
                stopwatch.Elapsed
            );
        }

        return response;
    }
}
