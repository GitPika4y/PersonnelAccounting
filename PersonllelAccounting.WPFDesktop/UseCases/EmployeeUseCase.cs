using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class EmployeeUseCase(IGenericCrudPaginationService<Employee> service): UseCaseBase, IEmployeeUseCase
{
    public async Task<Resource<PaginationModel<Employee>>> GetAllAsync(int page = 1, int pageSize = 10, Expression<Func<Employee, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllPagesAsync(filter, page, pageSize));


    public async Task<Resource> AddAsync(Employee employee) =>
        await SafeCallAsync(() => service.AddAsync(employee));

    public async Task<Resource> UpdateAsync(Employee employee) =>
        await SafeCallAsync(() => service.UpdateAsync(employee));
}