//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class DateTimePeriod : BaseValueObject
//{
//    protected DateTimePeriod()
//    {
//    }

//    private DateTimePeriod(DateTime startDateTime, DateTime? endDateTime)
//    {
//        StartDateTime = startDateTime;
//        EndDateTime = endDateTime;
//    }

//    public DateTime StartDateTime { get; private set; }
//    public DateTime? EndDateTime { get; private set; }

//    public static DateTimePeriod Create(DateTime startDateTime, DateTime? endDateTime)
//    {
//        if (startDateTime.IsOutOfSqlDateRange(out string? sqlStartDateErrorMessage))
//        {
//            throw new DomainException(sqlStartDateErrorMessage);
//        }

//        if (endDateTime.HasValue)
//        {
//            if (endDateTime.IsOutOfSqlDateRange(out string? sqlEndDateErrorMessage))
//            {
//                throw new DomainException(sqlEndDateErrorMessage);
//            }

//            if (endDateTime.IsEarlierThan(startDateTime, out string? earlierThanEndDateErrorMessage))
//            {
//                throw new DomainException(earlierThanEndDateErrorMessage);
//            }
//        }

//        return new DateTimePeriod(startDateTime, endDateTime);
//    }

//    public static implicit operator string(DateTimePeriod datePeriod)
//    {
//        return datePeriod.ToString();
//    }

//    public DateTimePeriod Shift(CountedValues day)
//    {
//        return new DateTimePeriod(StartDateTime.AddDays(day.Value), EndDateTime?.AddDays(day.Value));
//    }

//    public bool IsOver()
//    {
//        return EndDateTime.IsEarlierThanOrEqualTo(DateTime.Now);
//    }

//    public bool IsNotOver()
//    {
//        return EndDateTime.IsLaterThan(DateTime.Now);
//    }

//    public bool IsOver(DateTime dateTime)
//    {
//        return EndDateTime.IsEarlierThanOrEqualTo(dateTime);
//    }

//    public bool IsNotOver(DateTime dateTime)
//    {
//        return EndDateTime.IsLaterThan(dateTime);
//    }

//    public bool Contains(DateTime dateTime)
//    {
//        return dateTime.IsBetween(StartDateTime, EndDateTime);
//    }
    
//    public bool Within(DateTime dateTime)
//    {
//        return dateTime.IsBetweenDate(StartDateTime, EndDateTime);
//    }

//    public override string ToString()
//    {
//        return $"{StartDateTime:dd/MM/yyyy HH:mm:ss tt} ~ {EndDateTime:dd/MM/yyyy HH:mm:ss tt}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return StartDateTime;
//        yield return EndDateTime;
//    }
    
//    public bool HasStarted()
//    {
//        return StartDateTime <= DateTime.Now;
//    }

//    public DateTimePeriod CompareAndJustify(DateTimePeriod newPeriod)
//    {
//        DateTime? startDate = DateTimeUtility.GetLater(StartDateTime, newPeriod.StartDateTime);
//        DateTime? endDate = DateTimeUtility.GetEarlier(EndDateTime, newPeriod.EndDateTime);
//        return Create(startDate!.Value, endDate);
//    }
//}
