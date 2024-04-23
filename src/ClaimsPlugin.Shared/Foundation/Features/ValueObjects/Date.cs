//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Date : BaseValueObject
//{
//    protected Date()
//    {
//    }

//    private Date(DateTime value)
//    {
//        Value = value;
//    }

//    public DateTime Value { get; private set; }

//    public static Date Create(DateTime date)
//    {
//        if (date.Date.IsOutOfSqlDateRange(out string? sqlStartDateErrorMessage))
//        {
//            throw new DomainException(sqlStartDateErrorMessage);
//        }

//        return new Date(date.Date);
//    }

//    public static implicit operator DateTime(Date value)
//    {
//        return value.Value;
//    }

//    public static implicit operator string(Date date)
//    {
//        return date.ToString();
//    }

//    public override string ToString()
//    {
//        return Value.ToString("dd/MM/yyyy");
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return Value;
//    }
//}
