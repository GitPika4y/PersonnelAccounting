namespace Data.Models.Main;

public class Order: EntityModel
{
    public required Guid EmployeeId { get; set; }
    public required OrderType Type { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? HireDepartmentId { get; set; }
    public Guid? HirePositionId { get; set; }
    public string? FireReason { get; set; }

    public virtual OrderStatus Status
    {
        get
        {
            if (EndDate.HasValue && EndDate < DateTime.Now)
                return OrderStatus.Expired;

            return StartDate > DateTime.Now
                ? OrderStatus.Upcoming
                : OrderStatus.Active;
        }
    }

    public virtual Department? HireDepartment { get; set; }
    public virtual Position? HirePosition { get; set; }
    public virtual Employee Employee { get; set; } = null!;
}