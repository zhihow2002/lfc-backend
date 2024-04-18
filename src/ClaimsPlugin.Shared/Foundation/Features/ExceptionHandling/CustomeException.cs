using System.Net;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling;

public class CustomException : Exception
{
    public CustomException(
        string message,
        IEnumerable<string?>? errors = default,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError
    )
        : base(message)
    {
        ErrorMessages = errors?.Where(x => x.IsNotNullOrWhiteSpace()).Distinct().ToList();
        StatusCode = statusCode;
    }

    public List<string?>? ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }
}
