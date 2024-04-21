using ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Models;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Interceptors;

public class TenantInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation
    )
    {
        Metadata headers = context.Options.Headers ?? new Metadata();

        if (request is MultiTenantRequest multiTenantRequest)
        {
            string tenantId = multiTenantRequest.TenantId;
            if (!string.IsNullOrEmpty(tenantId))
            {
                headers.Add("tenant", tenantId);
            }
        }

        context = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            new CallOptions(headers)
        );

        return base.AsyncUnaryCall(request, context, continuation);
    }
}
