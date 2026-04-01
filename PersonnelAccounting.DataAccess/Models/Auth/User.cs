using System.ComponentModel.DataAnnotations;

namespace Data.Models.Auth;

public class User: EntityModel
{
    public required string Login { get; set; }

    [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
    public required string Password { get; set; }
    public required UserRole Role { get; set; }
    public string? Username { get; set; }
}