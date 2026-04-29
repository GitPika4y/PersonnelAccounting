using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class EmployeeUseCase(IGenericCrudPaginationService<Employee> service): UseCaseBase, IEmployeeUseCase
{
    public async Task<Resource<IReadOnlyCollection<Employee>>> GetAllAsync(Expression<Func<Employee, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllAsync(filter));

    public async Task<Resource<PaginationModel<Employee>>> GetAllPagesAsync(int page, int pageSize, Expression<Func<Employee, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllPagesAsync(page, pageSize, filter));

    public async Task<Resource<Employee?>> GetByIdAsync(Guid id) =>
        await SafeCallAsync(() => service.GetByIdAsync(id));

    public async Task<Resource> AddAsync(Employee employee) =>
        await SafeCallAsync(() => service.AddAsync(employee));

    public async Task<Resource> UpdateAsync(Employee employee) =>
        await SafeCallAsync(() => service.UpdateAsync(employee));
}