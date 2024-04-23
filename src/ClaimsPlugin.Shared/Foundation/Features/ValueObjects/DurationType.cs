//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class DurationType : BaseValueObject
//{
//    protected DurationType()
//    {
//    }

//    private DurationType(string value)
//    {
//        Value = value;
//    }

//    public static DurationType Second => new(nameof(Second));
//    public static DurationType Minute => new(nameof(Minute));
//    public static DurationType Hour => new(nameof(Hour));
//    public static DurationType Day => new(nameof(Day));
//    public static DurationType Week => new(nameof(Week));
//    public static DurationType Month => new(nameof(Month));
//    public static DurationType Year => new(nameof(Year));

//    public string Value { get; private set; } = default!;

//    public static IEnumerable<DurationType> SupportedItems
//    {
//        get
//        {
//            yield return Second;
//            yield return Minute;
//            yield return Hour;
//            yield return Day;
//            yield return Week;
//            yield return Month;
//            yield return Year;
//        }
//    }

//    public static DurationType From(string value)
//    {
//        DurationType item = new(value);

//        if (!SupportedItems.Contains(item))
//        {
//            throw new DomainException($"Unsupported {nameof(DurationType)}: {value}");
//        }

//        return item;
//    }

//    public static implicit operator string(DurationType value)
//    {
//        return value.ToString();
//    }

//    public static explicit operator DurationType(string value)
//    {
//        return From(value);
//    }

//    public override string ToString()
//    {
//        return Value;
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
