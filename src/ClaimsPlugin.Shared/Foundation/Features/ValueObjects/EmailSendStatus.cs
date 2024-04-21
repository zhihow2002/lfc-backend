using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class EmailSendStatus : BaseValueObject
{
    protected EmailSendStatus()
    {
    }

    private EmailSendStatus(string value)
    {
        Value = value;
    }

    public static EmailSendStatus Pending => new(nameof(Pending));
    public static EmailSendStatus Successful => new(nameof(Successful));

    public static EmailSendStatus Failed => new(nameof(Failed));
    public string Value { get; private set; } = default!;

    public static IEnumerable<EmailSendStatus> SupportedItems
    {
        get
        {
            yield return Pending;
            yield return Successful;
            yield return Failed;
        }
    }

    public static EmailSendStatus From(string value)
    {
        EmailSendStatus item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(EmailSendStatus)}: {value}");
        }

        return item;
    }

    public static implicit operator string(EmailSendStatus value)
    {
        return value.ToString();
    }

    public static explicit operator EmailSendStatus(string value)
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
