using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum Qualification
{
    [Display(Name = "Среднее")] Secondary,
    [Display(Name = "Среднее-специальное")] SecondarySpecial,
    [Display(Name = "Бакалавр")] Bachelor,
    [Display(Name = "Магистр")] Master,
    [Display(Name = "Специалист")] Specialist,
    [Display(Name = "Кандидат наук")] PhD
}