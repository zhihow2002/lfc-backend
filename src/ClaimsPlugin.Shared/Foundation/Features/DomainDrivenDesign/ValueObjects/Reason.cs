using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Reason : BaseValueObject
{
    protected Reason() { }

    private Reason(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static Reason Create(string reason)
    {
        if (reason.IsNullOrWhiteSpace(out string? messageNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(messageNullOrWhiteSpaceErrorMessage);
        }

        if (reason.HasLengthMoreThan(260, out string? contactMessageMaximumLengthErrorMessage))
        {
            throw new DomainException(contactMessageMaximumLengthErrorMessage);
        }

        return new Reason(reason);
    }

    public static implicit operator string(Reason message)
    {
        return message.ToString();
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
