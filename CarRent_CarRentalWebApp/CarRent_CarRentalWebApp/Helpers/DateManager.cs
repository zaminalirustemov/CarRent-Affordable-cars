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

}
