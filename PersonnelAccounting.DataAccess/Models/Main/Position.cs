using Microsoft.EntityFrameworkCore;

namespace Data.Models.Main;

[Index(nameof(Title), IsUnique = true)]
public class Position: EntityModel
{
    public required string Title { get; set; }
    public required decimal Salary { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = [];
}