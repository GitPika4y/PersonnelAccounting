using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IDepartmentUseCase
{
    Task<Resource<PaginationModel<Department>>> GetAllAsync(int page, int pageSize, Expression<Func<Department, bool>>? filter = null);
    Task<Resource<IReadOnlyCollection<Department>>> GetAllAsync(Expression<Func<Department, bool>>? filter = null);
    Task<Resource> AddAsync(Department department);
    Task<Resource> UpdateAsync(Department department);
}