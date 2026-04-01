using System.ComponentModel.DataAnnotations;

namespace Data.Models.Auth;

public enum UserRole
{
    [Display(Name="Админ")] Admin,
    [Display(Name="Пользователь")] User
}