using CarRent_CarRentalWebApp.Models;

namespace CarRent_CarRentalWebApp.Helpers;
public static class DateManager
{
    public static bool DateLogical(DateTime startDate, DateTime endDate)
    {
        if (startDate.CompareTo(DateTime.Now) < 0) return false;

        if (startDate >= endDate) return false;

        return true;
    }
    public static bool IntersectionTimeIntervals(DateTime interval1Start, DateTime interval1End, DateTime interval2Start, DateTime interval2End)
    {
        if (interval1Start >= interval2Start && interval1Start <= interval2End) return true;

        if (interval1End >= interval2Start && interval1End <= interval2End) return true;

        if (interval2Start >= interval1Start && interval2Start <= interval1End) return true;

        if (interval2End >= interval1Start && interval2End <= interval1End) return true;

        return false;
    }
    public static bool GreaterThan18(DateTime dateOfBirth)
    {
        return (dateOfBirth.AddYears(18) >= DateTime.Now);
    }
    public static string FindingTheZodiac(DateTime dateOfBirth)
    {
        int month = dateOfBirth.Month;
        int day = dateOfBirth.Day;
        switch (month)
        {
            case 1:
                if (day <= 19) return "Capricornus";
                else return "Aquarius";
            case 2:
                if (day <= 18) return "Aquarius";
                else return "Pisces";
            case 3:
                if (day <= 20) return "Pisces";
                else return "Aries";
            case 4:
                if (day <= 19) return "Aries";
                else return "Taurus";
            case 5:
                if (day <= 20) return "Taurus";
                else return "Gemini";
            case 6:
                if (day <= 20) return "Gemini";
                else return "Cancer";
            case 7:
                if (day <= 22) return "Cancer";
                else return "Leo";
            case 8:
                if (day <= 22) return "Leo";
                else return "Virgo";
            case 9:
                if (day <= 22) return "Virgo";
                else return "Libra";
            case 10:
                if (day <= 22) return "Libra";
                else return "Scorpion";
            case 11:
                if (day <= 21) return "Scorpion";
                else return "Sagittarius";
            case 12:
                if (day <= 21) return "Sagittarius";
                else return "Capricornus";
        }
        return "";
    }

}
