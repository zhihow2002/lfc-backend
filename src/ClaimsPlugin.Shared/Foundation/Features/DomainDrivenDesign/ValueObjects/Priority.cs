using System.Globalization;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Priority : BaseValueObject
{
    protected Priority() { }

    private Priority(int value)
    {
        Value = value;
    }

    public static Priority Default => new(0);
    public int Value { get; private set; }

    public static Priority Create(int value)
    {
        if (value.IsNegative(out string? valueNegativeErrormessage))
        {
            throw new DomainException(valueNegativeErrormessage);
        }

        return new Priority(value);
    }

    public static implicit operator int(Priority value)
    {
        return value.Value;
    }

    public static implicit operator string(Priority value)
    {
        return value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
