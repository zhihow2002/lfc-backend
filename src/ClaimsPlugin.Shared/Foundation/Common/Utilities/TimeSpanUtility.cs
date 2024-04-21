namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;

public static class TimeSpanUtility
{
    public static double GetTotalHoursBetweenTwoTimes(TimeSpan startTime, TimeSpan endTime)
    {
        return endTime.TotalHours - startTime.TotalHours;
    }

    public static double GetTotalMinutesBetweenTwoTimes(TimeSpan startTime, TimeSpan endTime)
    {
        return endTime.TotalMinutes - startTime.TotalMinutes;
    }

    public static List<TimeSpan> GetAllTimesBetweenTwoTimes(
        TimeSpan startTime,
        TimeSpan endTime,
        int step = 30
    )
    {
        TimeSpan diff = endTime - startTime;
        return Enumerable
            .Range(0, (int)(diff.TotalMinutes / step) + 1)
            .Select(x => startTime.Add(TimeSpan.FromMinutes(step * x)))
            .ToList();
    }
}
