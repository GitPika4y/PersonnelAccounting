using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IEmployeeUseCase
{
    Task<Resource<IReadOnlyCollection<Employee>>> GetAllAsync(Expression<Func<Employee, bool>>? filter = null);
    Task<Resource<PaginationModel<Employee>>> GetAllPagesAsync(int page, int pageSize, Expression<Func<Employee, bool>>? filter = null);
    Task<Resource<Employee?>> GetByIdAsync(Guid id);
    Task<Resource> AddAsync(Employee employee);
    Task<Resource> UpdateAsync(Employee employee);
}