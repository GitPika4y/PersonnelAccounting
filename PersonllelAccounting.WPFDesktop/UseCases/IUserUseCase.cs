using System.Linq.Expressions;
using Data.Models;
using Data.Models.Auth;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IUserUseCase
{
    Task<Resource<PaginationModel<User>>> GetAll(int page, int pageSize, Expression<Func<User, bool>>? filter = null);
    Task<Resource<IReadOnlyCollection<User>>> GetAll(Expression<Func<User, bool>>? filter = null);
    Task<Resource> Add(User user);
    Task<Resource> Update(User user);
}