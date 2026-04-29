using System.ComponentModel.DataAnnotations;

namespace WPF_Desktop.Utils;

public enum StatisticsEmployeeSeriesType
{
    [Display(Name = "Отделы")] Department,
    [Display(Name = "Должности")] Position
}