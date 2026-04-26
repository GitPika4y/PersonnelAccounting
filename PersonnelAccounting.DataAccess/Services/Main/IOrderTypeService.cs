using Data.Models.Main;

namespace Data.Services.Main;

public interface IOrderTypeService
{
    IEnumerable<OrderType> GetAvailable(Employee employee);
}