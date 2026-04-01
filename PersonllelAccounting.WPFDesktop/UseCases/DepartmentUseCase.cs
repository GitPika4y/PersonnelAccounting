using System.Linq.Expressions;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class DepartmentUseCase(IGenericCrudService<Department> service): UseCaseBase, IDepartmentUseCase
{
    public async Task<Resource<IReadOnlyCollection<Department>>> GetAllAsync() =>
        await SafeCallAsync(service.GetAllAsync);

    public async Task<Resource> AddAsync(Department department) =>
        await SafeCallAsync(() => service.AddAsync(department));

    public async Task<Resource> UpdateAsync(Department department) =>
        await SafeCallAsync(() => service.UpdateAsync(department));
}