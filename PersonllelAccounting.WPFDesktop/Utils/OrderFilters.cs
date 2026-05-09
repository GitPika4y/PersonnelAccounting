using Data.Models.Main;
using WPF_Desktop.Extensions;

namespace WPF_Desktop.Utils;

public class OrderFilters
{
    public string EmployeeName { get; set; }
    public OrderType OrderType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string BuildDescription()
    {
        var parts = new List<string>();

        parts.Add($"тип приказа ({OrderType.GetDisplayName()})");

        if (!string.IsNullOrWhiteSpace(EmployeeName))
            parts.Add($"поиск по сотруднику ({EmployeeName})");

        if (StartDate is not null)
            parts.Add($"дата создания после {StartDate.ToDateString()}");

        if (EndDate is not null)
            parts.Add($"дата окончания до {EndDate.ToDateString()}");

        return parts.Count == 0
            ? "Фильтры не применялись"
            : $"Приказы с использованием фильтров: [{string.Join(" | ", parts)}]";
    }
}