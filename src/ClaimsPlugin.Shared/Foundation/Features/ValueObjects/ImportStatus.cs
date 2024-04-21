using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class ImportStatus : BaseValueObject
{
    protected ImportStatus()
    {
    }

    private ImportStatus(string value)
    {
        Value = value;
    }

    public static ImportStatus Pending => new(nameof(Pending));
    public static ImportStatus Successful => new(nameof(Successful));
    public static ImportStatus Failed => new(nameof(Failed));
    public string Value { get; private set; } = default!;

    public static IEnumerable<ImportStatus> SupportedItems
    {
        get
        {
            yield return Pending;
            yield return Successful;
            yield return Failed;
        }
    }

    public static ImportStatus From(string value)
    {
        ImportStatus item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(ImportStatus)}: {value}");
        }

        return item;
    }

    public static implicit operator string(ImportStatus value)
    {
        return value.ToString();
    }

    public static explicit operator ImportStatus(string value)
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
