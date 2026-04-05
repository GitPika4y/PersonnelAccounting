using Data.Models.Main;

namespace TestProject1;

public class EmployeeTests
{
    private Employee SetUpEmployee()
    {
        return new Employee
        {
            Id = Guid.NewGuid(),
            LastName = "Тест",
            FirstName = "Тест",
            BirthDate = DateTime.Now - TimeSpan.FromDays(365 * 18),
            PhoneNumber = "8 323 900 90 90",
            Inn = "123456789012",
            Gender = EmployeeGender.Male
        };
    }

    public List<Order> SetUpOrdersWithFire(Employee employee, bool fireIsLast = true)
    {
        var hireOrder = new Order
        {
            EmployeeId = employee.Id,
            Type = OrderType.Hire,
            Date = DateTime.Now,
            StartDate = DateTime.Now.AddDays(1),
            Employee = employee,
            HireDepartment = new Department { Title = "DepartmentTest1" },
            HirePosition = new Position { Title = "PositionTest1", Salary = 1234 }
        };

        var fireOrder = new Order
        {
            EmployeeId = employee.Id,
            Type = OrderType.Fire,
            Date = DateTime.Now.AddDays(2),
            StartDate = DateTime.Now.AddDays(2),
            Employee = employee,
        };

        if (fireIsLast)
            return [hireOrder, fireOrder];

        var vacationOrder = new Order
        {
            EmployeeId = employee.Id,
            Type = OrderType.Vacation,
            Date = DateTime.Now.AddDays(3),
            StartDate = DateTime.Now.AddDays(3),
            Employee = employee,
        };

        return [hireOrder, fireOrder, vacationOrder];
    }

    public List<Order> SetUpOrdersWithoutFire(Employee employee)
    {
        var hireOrder = new Order
        {
            EmployeeId = employee.Id,
            Type = OrderType.Hire,
            Date = DateTime.Now,
            StartDate = DateTime.Now.AddDays(1),
            Employee = employee,
            HireDepartment = new Department { Title = "DepartmentTest1" },
            HirePosition = new Position { Title = "PositionTest1", Salary = 1234 }
        };

        var vacationOrder = new Order
        {
            EmployeeId = employee.Id,
            Type = OrderType.Vacation,
            Date = DateTime.Now.AddDays(3),
            StartDate = DateTime.Now.AddDays(3),
            Employee = employee,
        };

        return [hireOrder, vacationOrder];
    }

    [Fact]
    public void TestEmployeeStatusOnLastFireOrder()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithFire(employee);

        employee.Orders = orders;

        Assert.Equal(EmployeeStatus.Fired, employee.Status);
    }

    [Fact]
    public void BadTestEmployeeStatusOnFireNotLast()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithFire(employee, false);

        employee.Orders = orders;

        Assert.NotEqual(EmployeeStatus.Fired, employee.Status);
    }

    [Fact]
    public void TestEmployeeIsWorkingOnFireLast()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithFire(employee);

        employee.Orders = orders;

        Assert.False(employee.IsWorking);
    }

    [Fact]
    public void TestEmployeeIsWorkingOnFireLastNotLast()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithFire(employee, false);

        employee.Orders = orders;

        Assert.False(employee.IsWorking);
    }

    [Fact]
    public void TestEmployeeIsWorkingWithoutFire()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithoutFire(employee);
        employee.Orders = orders;
        Assert.True(employee.IsWorking);
    }

    [Fact]
    public void TestEmployeeIsWorkingWithoutOrders()
    {
        var employee = SetUpEmployee();
        Assert.False(employee.IsWorking);
    }

    [Fact]
    public void TestEmployeeHirePropertiesWithoutFire()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithoutFire(employee);
        employee.Orders = orders;

        Assert.NotNull(employee.HireOrder);
        Assert.NotNull(employee.Position);
        Assert.NotNull(employee.InWorkSince);
    }

    [Fact]
    public void TestEmployeeHirePropertiesWithFire()
    {
        var employee = SetUpEmployee();
        var orders = SetUpOrdersWithFire(employee);
        employee.Orders = orders;

        Assert.NotNull(employee.HireOrder);
        Assert.NotNull(employee.Position);
        Assert.NotNull(employee.InWorkSince);
    }
}