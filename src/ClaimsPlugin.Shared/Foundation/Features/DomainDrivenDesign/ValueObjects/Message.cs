using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Message : BaseValueObject
{
    protected Message() { }

    private Message(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static Message Create(string message)
    {
        if (message.IsNullOrWhiteSpace(out string? messageNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(messageNullOrWhiteSpaceErrorMessage);
        }

        if (message.HasLengthMoreThan(260, out string? contactMessageMaximumLengthErrorMessage))
        {
            throw new DomainException(contactMessageMaximumLengthErrorMessage);
        }

        return new Message(message);
    }

    public static implicit operator string(Message message)
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
