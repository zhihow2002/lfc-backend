using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class EmailRecipientType : BaseValueObject
{
    protected EmailRecipientType() { }

    private EmailRecipientType(string value)
    {
        Value = value;
    }

    public static EmailRecipientType To => new(nameof(To));
    public static EmailRecipientType Cc => new(nameof(Cc));

    public static EmailRecipientType Bcc => new(nameof(Bcc));
    public string Value { get; private set; } = default!;

    public static IEnumerable<EmailRecipientType> SupportedItems
    {
        get
        {
            yield return To;
            yield return Cc;
            yield return Bcc;
        }
    }

    public static EmailRecipientType From(string value)
    {
        EmailRecipientType item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(EmailRecipientType)}: {value}");
        }

        return item;
    }

    public static implicit operator string(EmailRecipientType value)
    {
        return value.ToString();
    }

    public static explicit operator EmailRecipientType(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
