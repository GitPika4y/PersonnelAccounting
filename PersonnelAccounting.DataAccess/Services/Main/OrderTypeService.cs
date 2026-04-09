using Data.Models.Main;

namespace Data.Services.Main;

public class OrderTypeService: IOrderTypeService
{
    public IEnumerable<OrderType> GetAvailable(Employee employee)
    {
        var orderTypes = Enum.GetValues<OrderType>();

        return employee.IsWorking ?
            orderTypes.Except([OrderType.Hire])
            : [OrderType.Hire];
    }
}