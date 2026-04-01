using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum EmployeeStatus
{
    [Display(Name="Не работает")] NotWorking,
    [Display(Name="Работает")] Working,
    [Display(Name="Уволен")] Fired,
    [Display(Name="В отпуске")] OnVacation,
    [Display(Name="В командировке")] OnBusinessTrip,
    [Display(Name="В ученическом отпуске")] OnStudyLeave
}