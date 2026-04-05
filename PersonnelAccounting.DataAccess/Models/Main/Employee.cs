using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Models.Main;

[Index(nameof(PhoneNumber), IsUnique = true)]
[Index(nameof(Inn), IsUnique = true)]
public class Employee: EntityModel
{
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Inn { get; set; }
    public required EmployeeGender Gender { get; set; }
    public Guid PassportId { get; set; }
    public Guid EducationId { get; set; }

    public virtual EmployeePassport Passport { get; set; } = null!;
    public virtual EmployeeEducation Education { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = [];

    public string FullName => $"{LastName} {FirstName} {MiddleName}";

    public bool IsWorking => Orders.Count != 0
                             && Orders.LastOrDefault(o => o.Type is OrderType.Hire or OrderType.Fire)
                                 ?.Type is OrderType.Hire;

    public EmployeeStatus Status
    {
        get
        {
            if (Orders.IsNullOrEmpty())
                return EmployeeStatus.NotWorking;

            var lastActiveOrder = Orders
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

    public DateTime? InWorkSince => Orders.LastOrDefault(o => o.Type is OrderType.Hire)
        ?.StartDate;

    public DateTime? InWorkUntil => Orders.LastOrDefault(o => o.Type is OrderType.Hire)
        ?.EndDate;

    public Position? Position => Orders.LastOrDefault(o => o.Type is OrderType.Hire)
        ?.HirePosition;

    public Order? HireOrder => Orders.LastOrDefault(o => o.Type is OrderType.Hire);

    public Order? FireOrder => Orders.LastOrDefault(o => o.Type is OrderType.Fire);
}