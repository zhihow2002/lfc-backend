using MediatR;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Feature
{
    // TODO: Proof of concept, to provide utility function in handler
    public abstract class FeatureHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken
        );
    }
}
