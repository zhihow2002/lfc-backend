//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class DayOfTheYear : BaseValueObject
//{
//    protected DayOfTheYear()
//    {
//    }

//    private DayOfTheYear(int value)
//    {
//        Value = value;
//    }

//    public static DayOfTheYear Default => new(1);
//    public int Value { get; private set; }

//    public static DayOfTheYear Create(int value)
//    {
//        if (value.IsNotBetween(1, 365, out string? valueNotBetweenErrorMessage))
//        {
//            throw new DomainException(valueNotBetweenErrorMessage);
//        }

//        return new DayOfTheYear(value);
//    }

//    public static implicit operator int(DayOfTheYear value)
//    {
//        return value.Value;
//    }

//    public static implicit operator string(DayOfTheYear value)
//    {
//        return value.ToString();
//    }

//    public override string ToString()
//    {
//        return Value.ToString();
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
