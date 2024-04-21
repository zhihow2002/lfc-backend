using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
public class ImportAction : BaseValueObject
{
    protected ImportAction() { }
    private ImportAction(string value)
    {
        Value = value;
    }
    public static ImportAction Add => new(nameof(Add));
    public static ImportAction Update => new(nameof(Update));
    public static ImportAction Delete => new(nameof(Delete));
    public static ImportAction None => new(nameof(None));
    public string Value { get; private set; } = default!;
    private static IEnumerable<ImportAction> SupportedItems
    {
        get
        {
            yield return Add;
            yield return Update;
            yield return Delete;
            yield return None;
        }
    }
    public static ImportAction From(string value)
    {
        ImportAction item = new(value);
        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(ImportAction)}: {value}");
        }
        return item;
    }
    public static implicit operator string(ImportAction value)
    {
        return value.ToString();
    }
    public static explicit operator ImportAction(string value)
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
