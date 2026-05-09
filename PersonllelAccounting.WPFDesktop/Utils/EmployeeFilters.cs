using Data.Models.Main;
using WPF_Desktop.Extensions;

namespace WPF_Desktop.Utils;

public class EmployeeFilters
{
    public string Name { get; set; }
    public EmployeeStatus Status { get; set; }

    public string BuildDescription()
    {
        List<string> parts = [];

        if (!string.IsNullOrEmpty(Name))
            parts.Add($"поиск по имени ({Name})");

        parts.Add($"статус ({Status.GetDisplayName()})");

        return $"Сотрудники с использованием фильтров: [{string.Join(" | ", parts)}]";
    }
}