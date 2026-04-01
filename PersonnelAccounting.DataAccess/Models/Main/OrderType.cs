using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum OrderType
{
    [Display(Name="Найм")] Hire,
    [Display(Name="Увольнение")] Fire,
    [Display(Name="Ученический отпуск")] StudyLeave,
    [Display(Name="Общий отпуск")] Vacation,
    [Display(Name="Командировка")] BusinessTrip
}