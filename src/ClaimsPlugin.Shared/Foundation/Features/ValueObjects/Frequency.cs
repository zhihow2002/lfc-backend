//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Frequency : BaseValueObject
//{
//    protected Frequency()
//    {
//    }

//    private Frequency(FrequencyType type, int day)
//    {
//        Type = type;
//        Day = day;
//    }

//    public FrequencyType Type { get; private set; } = default!;
//    public int Day { get; private set; }

//    public static Frequency Create(FrequencyType type, int day)
//    {
//        if (type == FrequencyType.Daily)
//        {
//            if (day.IsNotEqualTo(1))
//            {
//                throw new DomainException($"The expected day(s) for {type} should be 1.");
//            }
//        }
//        else if (type == FrequencyType.Weekly)
//        {
//            if (day.IsNotBetween(1, 7))
//            {
//                throw new DomainException($"The expected day(s) for {type} should between 1 to 7.");
//            }
//        }
//        else if (type == FrequencyType.Monthly)
//        {
//            if (day.IsNotBetween(1, 31))
//            {
//                throw new DomainException($"The expected day(s) for {type} should between 1 to 31.");
//            }
//        }
//        else if (type == FrequencyType.Yearly)
//        {
//            if (day.IsNotBetween(1, 366))
//            {
//                throw new DomainException($"The expected day(s) for {type} should between 1 to 366.");
//            }
//        }
//        else
//        {
//            throw new DomainException($"{type} is not supported.");
//        }

//        return new Frequency(type, day);
//    }

//    public static implicit operator string(Frequency amountRange)
//    {
//        return amountRange.ToString();
//    }

//    public override string ToString()
//    {
//        return $"On the {Day} day, {Type?.Value}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Type;
//        yield return Day;
//    }

//    public bool IsTodayTheLastDay()
//    {
//        if (Type == FrequencyType.Daily)
//        {
//            return true;
//        }
        
//        if (Type == FrequencyType.Weekly)
//        {
//            return Day == 7 && DateTime.Today.DayOfWeek == DayOfWeek.Sunday;
//        }
        
//        if (Type == FrequencyType.Monthly)
//        {
//            DateTime today = DateTime.Today;
//            return Day >= DateTime.DaysInMonth(today.Year, today.Month);
//        }
        
//        if (Type == FrequencyType.Yearly)
//        {
//            DateTime beginOfYear = new(DateTime.Today.Year, 01, 01, 0, 0, 0, DateTimeKind.Local);
//            DateTime endOfYear = beginOfYear.AddYears(1).AddDays(-1);
//            double daysOfYear = endOfYear.Subtract(beginOfYear).TotalDays;
//            return Day >= daysOfYear;
//        }
        
//        return false;
//    }
    
//    public bool IsMatchDate(DateTime paymentBatchDate)
//    {
//        if (Type == FrequencyType.Daily)
//        {
//            return true;
//        }

//        int todayDay = (int)paymentBatchDate.DayOfWeek;
//        todayDay = todayDay == 0 ? 7 : todayDay;

//        if (Type == FrequencyType.Weekly && todayDay == Day)
//        {
//            return true;
//        }
        
//        int lastDayOfMonth = DateTime.DaysInMonth(paymentBatchDate.Year, paymentBatchDate.Month);

//        if (Type == FrequencyType.Monthly && (paymentBatchDate.Day == Day ||
//                                              (Day > lastDayOfMonth && paymentBatchDate.Day == lastDayOfMonth)))
//        {
//            return true;
//        }
        
//        int daysThisYear = new DateTime(paymentBatchDate.Year + 1, 1, 1).AddDays(-1).DayOfYear;
        
//        if (Type == FrequencyType.Yearly && (paymentBatchDate.DayOfYear == Day ||
//                                             (Day > daysThisYear && paymentBatchDate.DayOfYear == daysThisYear)))
//        {
//            return true;
//        }

//        return false;
//    }
//}
