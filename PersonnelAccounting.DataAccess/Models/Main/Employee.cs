using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Models.Main;

[Index(nameof(PhoneNumber), IsUnique = true)]
[Index(nameof(Passport), IsUnique = true)]
[Index(nameof(Inn), IsUnique = true)]
public class Employee: EntityModel
{
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Inn { get; set; }
    public required string Passport { get; set; }

    public virtual EmployeeStatus Status
    {
        get
        {
            if (Orders.IsNullOrEmpty())
                return EmployeeStatus.NotWorking;

            var lastActiveOrder = Orders
                .Where(o => o.Status is OrderStatus.Active)
                .OrderByDescending(o => o.StartDate)
                .Take(1)
                .FirstOrDefault();

            if (lastActiveOrder is null)
                return EmployeeStatus.NotWorking;

            var lastOrderType = lastActiveOrder.Type;

            return lastOrderType switch
            {
                OrderType.Hire => EmployeeStatus.Working,
                OrderType.Fire => EmployeeStatus.Fired,
                OrderType.StudyLeave => EmployeeStatus.OnStudyLeave,
                OrderType.Vacation => EmployeeStatus.OnVacation,
                OrderType.BusinessTrip => EmployeeStatus.OnBusinessTrip,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public virtual ICollection<Order> Orders { get; set; } = [];
}