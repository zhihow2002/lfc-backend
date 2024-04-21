using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class EmailAddress : BaseValueObject
{
    protected EmailAddress()
    {
    }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static EmailAddress Create(string email)
    {
        if (email.IsNullOrWhiteSpace(out string? emailNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(emailNullOrWhiteSpaceErrorMessage);
        }

        if (email.IsNotEmail(out string? emailInvalidErrorMessage))
        {
            throw new DomainException(emailInvalidErrorMessage);
        }

        if (email.HasLengthMoreThan(320, out string? contactEmailMaximumLengthErrorMessage))
        {
            throw new DomainException(contactEmailMaximumLengthErrorMessage);
        }

        return new EmailAddress(email);
    }

    public static implicit operator string(EmailAddress email)
    {
        return email.ToString();
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
