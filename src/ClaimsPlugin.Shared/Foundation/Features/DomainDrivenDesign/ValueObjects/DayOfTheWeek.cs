using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
public class DayOfTheWeek : BaseValueObject
{
    protected DayOfTheWeek() { }
    private DayOfTheWeek(int value)
    {
        Value = value;
    }
    public static DayOfTheWeek Default => new(1);
    public int Value { get; private set; }
    public static DayOfTheWeek Create(int value)
    {
        if (value.IsNotBetween(1, 7, out string? valueNotBetweenErrorMessage))
        {
            throw new DomainException(valueNotBetweenErrorMessage);
        }
        return new DayOfTheWeek(value);
    }
    public static implicit operator int(DayOfTheWeek value)
    {
        return value.Value;
    }
    public static implicit operator string(DayOfTheWeek value)
    {
        return value.ToString();
    }
    public override string ToString()
    {
        return Value switch
        {
            1 => "Monday",
            2 => "Tuesday",
            3 => "Wednesday",
            4 => "Thursday",
            5 => "Friday",
            6 => "Saturday",
            7 => "Sunday",
            _ => throw new DomainException($"Day of week '{Value}' is unsupported.")
        };
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
