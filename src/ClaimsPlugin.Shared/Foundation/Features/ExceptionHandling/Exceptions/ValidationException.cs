using System.Net;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class ValidationException : CustomException
{
    public ValidationException(string? errorMessage) : base(
        string.Empty,
        !string.IsNullOrWhiteSpace(errorMessage) ? new List<string?> { errorMessage } : null,
        HttpStatusCode.BadRequest
    )
    {
    }

    public ValidationException(List<string?>? errors = default) : base(string.Empty, errors, HttpStatusCode.BadRequest)
    {
    }

    public ValidationException(params string?[] errors) : base(string.Empty, errors.Where(x => x.IsNotNullOrWhiteSpace()).ToList(), HttpStatusCode.BadRequest)
    {
    }
}
