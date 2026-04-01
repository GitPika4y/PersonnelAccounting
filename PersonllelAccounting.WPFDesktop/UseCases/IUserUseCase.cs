using System.Linq.Expressions;
using Data.Models.Auth;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IUserUseCase
{
    Task<Resource<IReadOnlyCollection<User>>> GetAll();
    Task<Resource<IReadOnlyCollection<User>>> GetAll(Expression<Func<User, bool>> filter);
    Task<Resource> Add(User user);
    Task<Resource> Update(User user);
}