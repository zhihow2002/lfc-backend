//using System.Text;
//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class Age : BaseValueObject
//{
//    protected Age()
//    {
//    }

//    private Age(int year, int month, int day)
//    {
//        Year = year;
//        Month = month;
//        Day = day;
//    }

//    public int TotalDays => (int)this;
//    public int Current => GetCurrentAge(); 
//    public int Year { get; private set; }
//    public int Month { get; private set; }
//    public int Day { get; private set; }

//    public static Age Create(int year)
//    {
//        return Create(year, 1, 1);
//    }
    
//    public static Age Create(int year, int month)
//    {
//        return Create(year, month, 1);
//    }
    
//    public static Age Create(int year, int month, int day)
//    {
//        if (year.IsNegative(out string? yearNegativeErrormessage))
//        {
//            throw new DomainException(yearNegativeErrormessage);
//        }

//        if (month.IsNegative(out string? monthNegativeErrormessage))
//        {
//            throw new DomainException(monthNegativeErrormessage);
//        }
        
//        if (day.IsNegative(out string? dayNegativeErrormessage))
//        {
//            throw new DomainException(dayNegativeErrormessage);
//        }

//        return new Age(year, month, day);
//    }

//    public static Age Create(DateTime dateOfBirth)
//    {
//        return new Age(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
//    }

//    public static implicit operator string(Age value)
//    {
//        return value.ToString();
//    }

//    public static implicit operator int(Age value)
//    {
//        int totalDays = value.Day;

//        for (int month = 1; month < value.Month; month++)
//        {
//            totalDays += DateTime.DaysInMonth(value.Year, month);
//        }
        
//        if (IsLeapYear(value.Year))
//        {
//            totalDays++; // Account for the leap day in a leap year after February
//        }

//        if (IsLunarYear(value.Year))
//        {
//            totalDays++; // Account for an extra day in a lunar year
//        }

//        return totalDays;
//    }

//    private int GetCurrentAge()
//    {
//        int currentYear = DateTime.Now.Year;
//        int age = currentYear - Year;
        
//        DateTime birthDate = new(Year, Month, Day);
//        DateTime currentDate = DateTime.Now;

//        if (birthDate.Month > currentDate.Month || 
//            (birthDate.Month == currentDate.Month && birthDate.Day > currentDate.Day))
//        {
//            age--;
//        }

//        return age;
//    }

//    private static bool IsLeapYear(int year)
//    {
//        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
//    }

//    private static bool IsLunarYear(int year)
//    {
//        return (year % 19 == 0) || (year % 19 == 3) || (year % 19 == 6) || (year % 19 == 9) || (year % 19 == 11) || (year % 19 == 14) || (year % 19 == 17);
//    }
    
//    public override string ToString()
//    {
//        StringBuilder sb = new();

//        if (Year.IsNotZero())
//        {
//            sb.Append($"{Year} year{(Year > 1 ? "s" : "")} ");
//        }

//        if (Month.IsNotZero())
//        {
//            sb.Append($"{Month} month{(Month > 1 ? "s" : "")} ");
//        }

//        if (Day.IsNotZero())
//        {
//            sb.Append($"{Day} day{(Day > 1 ? "s" : "")} ");
//        }

//        string result = sb.ToString().Trim();

//        return result;
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Year;
//        yield return Month;
//        yield return Day;
//    }
//}
