using System.Globalization;

namespace WPF_Desktop.Extensions;

public static class DateTimeExtensions
{
    public static string ToDateString(this DateTime date)
    {
        return date.ToString("dd.MM.yyyy");
    }

    public static string ToDateString(this DateTime? date)
    {
        return date?.ToString("dd.MM.yyyy") ?? "Бессрочно";
    }
}