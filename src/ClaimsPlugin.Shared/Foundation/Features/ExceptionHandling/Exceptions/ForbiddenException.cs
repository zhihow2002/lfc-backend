using System.Net;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : base(message, null, HttpStatusCode.Forbidden)
    {
    }
}
