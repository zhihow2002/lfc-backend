using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
public class DatePeriod : BaseValueObject
{
    protected DatePeriod() { }
    private DatePeriod(DateTime startDate, DateTime? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DatePeriod Duplicate()
    {
        return Create(StartDate, EndDate);
    }
    public static DatePeriod Create(DateTime startDate, DateTime? endDate)
    {
        if (startDate.Date.IsOutOfSqlDateRange(out string? sqlStartDateErrorMessage))
        {
            throw new DomainException(sqlStartDateErrorMessage);
        }
        if (endDate.HasValue)
        {
            if (endDate.Value.Date.IsOutOfSqlDateRange(out string? sqlEndDateErrorMessage))
            {
                throw new DomainException(sqlEndDateErrorMessage);
            }
            if (
                endDate.Value.Date.IsEarlierThan(
                    startDate.Date,
                    out string? earlierThanEndDateErrorMessage
                )
            )
            {
                throw new DomainException(earlierThanEndDateErrorMessage);
            }
        }
        startDate = startDate.Date;
        endDate = endDate?.Date;
        return new DatePeriod(startDate, endDate);
    }
    public static implicit operator string(DatePeriod datePeriod)
    {
        return datePeriod.ToString();
    }
    public bool IsOver()
    {
        return EndDate?.IsEarlierThanOrEqualTo(DateTime.Now.Date) ?? false;
    }
    public bool IsNotOver()
    {
        return EndDate?.IsLaterThan(DateTime.Now.Date) ?? true;
    }
    public bool IsOver(DateTime dateTime)
    {
        return EndDate?.IsEarlierThanOrEqualTo(dateTime) ?? false;
    }
    public bool IsNotOver(DateTime dateTime)
    {
        return EndDate?.IsLaterThan(dateTime) ?? true;
    }
    public bool Contains(DateTime dateTime)
    {
        return dateTime.IsBetween(StartDate.Date, EndDate?.Date);
    }
    public override string ToString()
    {
        return $"{StartDate:dd/MM/yyyy} ~ {EndDate:dd/MM/yyyy}";
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
