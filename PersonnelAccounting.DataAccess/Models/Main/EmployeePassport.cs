using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Models.Main;

[Index(nameof(Serial), IsUnique = true)]
[Index(nameof(Number), IsUnique = true)]
public class EmployeePassport: EntityModel
{
    [RegularExpression(@"^\d{4}$")]
    public required string Serial { get; set; }
    [RegularExpression(@"^\d{6}$")]
    public required string Number { get; set; }
    public required DateTime Date { get; set; }
    public required string GivenBy { get; set; }

    public virtual Employee Employee { get; set; }
}