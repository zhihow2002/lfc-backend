using System.Net;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Models;
using ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Grpc.Core;
using Microsoft.Extensions.Localization;
using Serilog;
using Serilog.Context;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentIdentity _currentIdentity;
    private readonly ISerializerService _jsonSerializer;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

    public ExceptionMiddleware(
        ICurrentIdentity currentIdentity,
        IStringLocalizer<ExceptionMiddleware> localizer,
        ISerializerService jsonSerializer
    )
    {
        _currentIdentity = currentIdentity;
        _localizer = localizer;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            Guid identityId = _currentIdentity.GetIdentityId();

            if (identityId != Guid.Empty)
            {
                LogContext.PushProperty("IdentityId", identityId);
            }

            string traceId = Guid.NewGuid().ToString();

            LogContext.PushProperty("TraceId", traceId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);

            ErrorResult errorResult =
                new()
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                    TraceId = traceId,
                    IsSuccess = false
                };

            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case ValidationException e:
                    errorResult.Title = "One or more validation errors occurred.";
                    errorResult.StatusCode = (int)e.StatusCode;

                    if (e.ErrorMessages.IsNotNull())
                    {
                        errorResult.Messages = e.ErrorMessages!;
                    }

                    break;

                case DomainException e:
                    errorResult.Title = "One or more domain errors occurred.";
                    errorResult.StatusCode = (int)e.StatusCode;

                    if (e.ErrorMessages.IsNotNull())
                    {
                        errorResult.Messages = e.ErrorMessages!;
                    }

                    break;

                case ActionRequiredException e:
                    errorResult.Title = "One or more action required.";
                    errorResult.StatusCode = (int)e.StatusCode;
                    errorResult.SupportCode = e.SupportCode;
                    errorResult.SupportMessage = e.SupportMessage;

                    if (e.ErrorMessages.IsNotNull())
                    {
                        errorResult.Messages = e.ErrorMessages!;
                    }

                    break;

                case CustomException e:
                    errorResult.Title = "One or more custom errors occurred.";
                    errorResult.StatusCode = (int)e.StatusCode;

                    if (e.ErrorMessages.IsNotNull())
                    {
                        errorResult.Messages = e.ErrorMessages!;
                    }

                    break;

                case RpcException e:
                    errorResult.Title = "One or more grpc errors occurred.";
                    errorResult.StatusCode = (int)HttpStatusCode.BadRequest;

                    if (e.Status.Detail.IsNotNull())
                    {
                        errorResult.Messages = new List<string> { e.Status.Detail };
                    }

                    break;

                case KeyNotFoundException:
                    errorResult.Title = "Not found.";
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorResult.Title = "One or more internal server errors occurred.";
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResult.Messages.Add(exception.Message);
                    errorResult.SupportMessage = _localizer["exceptionMiddleware.supportMessage"];
                    break;
            }

            HttpResponse response = context.Response;

            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;

                string serialized = _jsonSerializer.Serialize(
                    errorResult,
                    escapeNonAsciiCharacters: false
                );

                Log.Error(
                    "HTTP {Method} Request failed with Status Code {StatusCode}, Uri: {Request}, Trace Id: {TraceId}, Error: {ErrorResult}",
                    context.Request.Method,
                    context.Response.StatusCode,
                    $"{context.Request.Scheme}://{context.Request.Host.Value}{(context.Request.PathBase.HasValue ? $"/{context.Request.PathBase.Value}" : string.Empty)}{context.Request.Path}{context.Request.QueryString}",
                    traceId,
                    serialized
                );

                await response.WriteAsync(serialized);
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
