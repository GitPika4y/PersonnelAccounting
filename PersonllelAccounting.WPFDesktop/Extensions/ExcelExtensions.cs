using System.Globalization;
using ClosedXML.Excel;

namespace WPF_Desktop.Extensions;

public static class ExcelExtensions
{
    public static void SetValue(this IXLCell cell, object? value)
    {
        cell.Value = value switch
        {
            int i => i.ToString(),
            double i => i.ToString(CultureInfo.InvariantCulture),
            Enum e => e.GetDisplayName(),
            Guid g => g.ToString(),
            string s => s,
            DateTime dt => dt.ToDateString(),
            null => "",
            bool b => b.ToString(),
            decimal d => d.ToString(CultureInfo.InvariantCulture),
            _ => value.ToString() ?? ""
        };
    }
}