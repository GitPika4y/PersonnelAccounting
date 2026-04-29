using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class OrderUseCase(IGenericCrudPaginationService<Order> service): UseCaseBase, IOrderUseCase
{
    public async Task<Resource<IReadOnlyCollection<Order>>> GetAllAsync(Expression<Func<Order, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllAsync(filter));

    public async Task<Resource<PaginationModel<Order>>> GetAllAsync(int page, int pageSize, Expression<Func<Order, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllPagesAsync(page, pageSize, filter));

    public async Task<Resource> AddAsync(Order order) =>
        await SafeCallAsync(() => service.AddAsync(order));


    public async Task<Resource> UpdateAsync(Order order) =>
        await SafeCallAsync(() => service.UpdateAsync(order));
}