using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IOrderUseCase
{
    Task<Resource<PaginationModel<Order>>> GetAllAsync(int page, int pageSize, Expression<Func<Order, bool>>? filter = null);
    Task<Resource> AddAsync(Order order);
    Task<Resource> UpdateAsync(Order order);
}