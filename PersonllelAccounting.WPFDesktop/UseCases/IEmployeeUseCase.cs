using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IEmployeeUseCase
{
    Task<Resource<PaginationModel<Employee>>> GetAllAsync(int page = 1, int pageSize = 10, Expression<Func<Employee, bool>>? filter = null);
    Task<Resource> AddAsync(Employee employee);
    Task<Resource> UpdateAsync(Employee employee);
}