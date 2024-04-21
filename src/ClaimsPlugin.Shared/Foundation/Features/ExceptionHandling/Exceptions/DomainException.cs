using System.Net;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class DomainException : CustomException
{
    public DomainException(string? errorMessage)
        : base(
            string.Empty,
            !string.IsNullOrWhiteSpace(errorMessage) ? new List<string?> { errorMessage } : null,
            HttpStatusCode.BadRequest
        ) { }

    public DomainException(List<string?>? errors = default)
        : base(string.Empty, errors, HttpStatusCode.BadRequest) { }

    public DomainException(params string?[] errors)
        : base(
            string.Empty,
            errors.Where(x => x.IsNotNullOrWhiteSpace()).ToList(),
            HttpStatusCode.BadRequest
        ) { }
}
