using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Models.Main;

[Index(nameof(PhoneNumber), IsUnique = true)]
[Index(nameof(Inn), IsUnique = true)]
public class Employee : EntityModel
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

    public virtual required EmployeePassport Passport { get; set; } = null!;
    public virtual required EmployeeEducation Education { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = [];

    private IEnumerable<Order> OrderedOrders =>
        Orders.OrderByDescending(o => o.StartDate);

    private Order? GetLastOrder(Func<Order, bool>? predicate = null)
    {
        var query = OrderedOrders;

        if (predicate != null)
            query = query.Where(predicate);

        return query.FirstOrDefault();
    }

    public string FullName =>
        $"{LastName} {FirstName} {MiddleName}".Trim();

    public bool IsWorking =>
        GetLastOrder(o => o.Type is OrderType.Hire or OrderType.Fire)?.Type
            == OrderType.Hire;

    public EmployeeStatus Status
    {
        get
        {
            var lastOrder = GetLastOrder(o => o.Status is OrderStatus.Active);

            if (lastOrder == null)
                return EmployeeStatus.NotWorking;

            return lastOrder.Type switch
            {
                OrderType.Hire => EmployeeStatus.Working,
                OrderType.Fire => EmployeeStatus.Fired,
                OrderType.StudyLeave => EmployeeStatus.OnStudyLeave,
                OrderType.Vacation => EmployeeStatus.OnVacation,
                OrderType.BusinessTrip => EmployeeStatus.OnBusinessTrip,
                _ => EmployeeStatus.NotWorking
            };
        }
    }

    public Order? HireOrder =>
        GetLastOrder(o => o.Type == OrderType.Hire);

    public Order? FireOrder =>
        GetLastOrder(o => o.Type == OrderType.Fire);

    public DateTime? InWorkSince =>
        HireOrder?.StartDate;

    public DateTime? InWorkUntil =>
        HireOrder?.EndDate;

    public Position? Position =>
        HireOrder?.HirePosition;
}