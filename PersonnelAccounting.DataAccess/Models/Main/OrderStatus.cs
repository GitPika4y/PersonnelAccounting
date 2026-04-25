using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum OrderStatus
{
    [Display(Name="Просрочен")] Expired,
    [Display(Name="Намечающийся")] Upcoming,
    [Display(Name="Активный")] Active
}