using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum EmployeeGender
{
    [Display(Name="Муж")] Male,
    [Display(Name="Жен")] Female
}