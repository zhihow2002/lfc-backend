namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

public class InternalServerException : CustomException
{
    public InternalServerException(string message, List<string>? errors = default)
        : base(message, errors)
    {
    }

    public InternalServerException(List<string>? errors = default)
        : base(string.Empty, errors)
    {
    }
}
