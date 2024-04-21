using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;

public static class DateTimeUtility
{
    public static double GetTotalDaysBetweenTwoDates(DateTime startDate, DateTime endDate)
    {
        return (endDate.Date - startDate.Date).TotalDays;
    }

    public static double GetTotalDaysBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return (endDate.Value.Date - startDate.Date).TotalDays;
    }

    public static double GetTotalHoursBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return (endDate.Value - startDate).TotalHours;
    }

    public static double GetTotalMinutesBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return (endDate.Value - startDate).TotalMinutes;
    }

    public static double GetTotalSecondsBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return (endDate.Value - startDate).TotalSeconds;
    }

    public static double GetTotalYearsBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return endDate.Value.Year
            - startDate.Year
            - 1
            + (
                endDate.Value.Month > startDate.Month
                || (endDate.Value.Month == startDate.Month && endDate.Value.Day >= startDate.Day)
                    ? 1
                    : 0
            );
    }

    public static List<DateTime> GetAllDatesBetweenTwoDates(DateTime startDate, DateTime? endDate)
    {
        if (endDate.IsNull())
        {
            endDate = DateTime.MaxValue;
        }
        return Enumerable
            .Range(0, 1 + endDate.Value.Subtract(startDate).Days)
            .Select(offset => startDate.AddDays(offset))
            .ToList();
    }

    public static DateTime GetNextWorkingDateTime(DateTime givenDate, List<DateTime> holidayList)
    {
        givenDate = givenDate.Date;
        do
        {
            givenDate = givenDate.AddDays(1);
        } while (IsHoliday(givenDate, holidayList) || IsWeekend(givenDate));
        return givenDate;
    }

    private static bool IsHoliday(DateTime date, List<DateTime> holidayList)
    {
        List<DateTime> list = holidayList.Select(x => x.Date).ToList();
        return list.Contains(date);
    }

    private static bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static bool IsOverlap(
        DateTime startA,
        DateTime? endDateA,
        DateTime startB,
        DateTime? endDateB
    )
    {
        DateTime endA = endDateA.IsNull() ? DateTime.MaxValue : endDateA.Value;
        DateTime endB = endDateB.IsNull() ? DateTime.MaxValue : endDateB.Value;
        return (startA.Date >= startB.Date && startA.Date <= endB.Date)
            || (endA.Date >= startB.Date && endA.Date <= endB.Date)
            || (startA.Date < startB.Date && endA.Date > endB.Date);
    }

    public static DateTime? GetEarlier(DateTime? dateA, DateTime? dateB)
    {
        if (dateA.IsNull() && dateB.IsNull())
        {
            return null;
        }
        if (dateA.IsNotNull() && dateB.IsNotNull())
        {
            return dateA.Value.Date < dateB.Value.Date ? dateA : dateB;
        }
        return dateA.IsNull() ? dateB : dateA;
    }

    public static DateTime? GetLater(DateTime? dateA, DateTime? dateB)
    {
        if (dateA.IsNull() && dateB.IsNull())
        {
            return null;
        }
        if (dateA.IsNotNull() && dateB.IsNotNull())
        {
            return dateA.Value.Date > dateB.Value.Date ? dateA : dateB;
        }
        return dateA.IsNull() ? dateA : dateB;
    }

    public static string StringFormat(
        DateTime? date,
        bool addBracket = true,
        string format = "dd/MM/yyyy"
    )
    {
        if (date.IsNull())
        {
            return "";
        }
        return addBracket ? "(" + date.Value.ToString(format) + ")" : date.Value.ToString(format);
    }
}
