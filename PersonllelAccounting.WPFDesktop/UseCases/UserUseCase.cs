using System.Linq.Expressions;
using Data.Models.Auth;
using Data.Services.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class UserUseCase(IUserService service): UseCaseBase, IUserUseCase
{
    public async Task<Resource<IReadOnlyCollection<User>>> GetAll(Expression<Func<User, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllAsync(filter));

    public async Task<Resource> Add(User user) =>
        await SafeCallAsync(() => service.AddAsync(user));


    public async Task<Resource> Update(User user) =>
        await SafeCallAsync(() => service.UpdateAsync(user));
}