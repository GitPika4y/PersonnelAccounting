using System.Linq.Expressions;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IPositionUseCase
{
    Task<Resource<PaginationModel<Position>>> GetAllAsync(int page, int pageSize, Expression<Func<Position, bool>>? filter = null);
    Task<Resource<IReadOnlyCollection<Position>>> GetAllAsync(Expression<Func<Position, bool>>? filter = null);
    Task<Resource> AddAsync(Position position);
    Task<Resource> UpdateAsync(Position position);
}