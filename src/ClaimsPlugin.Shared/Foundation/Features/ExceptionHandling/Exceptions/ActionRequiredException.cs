using System.Net;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
public class ActionRequiredException : CustomException
{
    public string SupportCode { get; private set; }
    public string? SupportMessage { get; private set; }
    public ActionRequiredException(string supportCode, string? supportMessage, string? errorMessage) : base(
        string.Empty,
        !string.IsNullOrWhiteSpace(errorMessage) ? new List<string?> { errorMessage } : null,
        HttpStatusCode.Conflict
    )
    {
        SupportCode = supportCode;
        SupportMessage = supportMessage;
    }
    public ActionRequiredException(string supportCode, string? supportMessage, List<string?>? errors = default) : base(string.Empty, errors, HttpStatusCode.Conflict)
    {
        SupportCode = supportCode;
        SupportMessage = supportMessage;
    }
    public ActionRequiredException(string supportCode, string? supportMessage, params string?[] errors) : base(string.Empty, errors.Where(x => x.IsNotNullOrWhiteSpace()).ToList(), HttpStatusCode.Conflict)
    {
        SupportCode = supportCode;
        SupportMessage = supportMessage;
    }
}
