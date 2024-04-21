using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class EmailSubject : BaseValueObject
{
    protected EmailSubject() { }

    private EmailSubject(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static EmailSubject Create(string emailSubject)
    {
        if (emailSubject.IsNullOrWhiteSpace(out string? emailSubjectNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(emailSubjectNullOrWhiteSpaceErrorMessage);
        }

        if (emailSubject.HasLengthMoreThan(320, out string? emailSubjectMaximumLengthErrorMessage))
        {
            throw new DomainException(emailSubjectMaximumLengthErrorMessage);
        }

        return new EmailSubject(emailSubject);
    }

    public static implicit operator string(EmailSubject emailSubject)
    {
        return emailSubject.ToString();
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
