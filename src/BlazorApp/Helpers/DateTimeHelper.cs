namespace BlazorApp.Helpers;

public static class DateTimeHelper
{
    public static string GetDuration(DateTime startDate, DateTime? endDate)
    {
        var end = endDate ?? DateTime.UtcNow;
        var duration = end - startDate;
        var years = duration.Days / 365;
        var months = (duration.Days % 365) / 30;

        if (years > 0 && months > 0)
            return $"{years} yr{(years != 1 ? "s" : "")} {months} mo{(months != 1 ? "s" : "")}";
        if (years > 0)
            return $"{years} yr{(years != 1 ? "s" : "")}";
        if (months > 0)
            return $"{months} mo{(months != 1 ? "s" : "")}";
        return "Less than 1 month";
    }
}
