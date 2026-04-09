using System.Linq.Expressions;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class DepartmentUseCase(IGenericCrudService<Department> service): UseCaseBase, IDepartmentUseCase
{
    public async Task<Resource<IReadOnlyCollection<Department>>> GetAllAsync(Expression<Func<Department, bool>>? filter) =>
        await SafeCallAsync(() => service.GetAllAsync(filter));

    public async Task<Resource> AddAsync(Department department) =>
        await SafeCallAsync(() => service.AddAsync(department));

    public async Task<Resource> UpdateAsync(Department department) =>
        await SafeCallAsync(() => service.UpdateAsync(department));
}