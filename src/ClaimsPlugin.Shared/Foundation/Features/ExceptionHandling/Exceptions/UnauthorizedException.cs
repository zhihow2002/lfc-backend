using System.Net;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
        : base(message, null, HttpStatusCode.Unauthorized)
    {
    }
}
