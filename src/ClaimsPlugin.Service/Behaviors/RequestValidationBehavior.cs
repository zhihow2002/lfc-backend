// using FluentValidation;
// using Mediator;

// namespace ClaimsPlugin.Repository.Behaviors
// {
//     public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//         where TRequest : IRequest<TResponse>
//     {
//         private readonly IEnumerable<IValidator<TRequest>> _validators;

//         public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
//         {
//             _validators = validators;
//         }

//         public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
//         {
//             var context = new ValidationContext<TRequest>(message);
//             var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
//             var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

//             if (failures?.Count() > 0)
//             {
//                 throw new ValidationException(failures);
//             }

//             return await next(message, cancellationToken);
//         }
//     }
// }
