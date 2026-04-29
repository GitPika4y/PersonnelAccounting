using System.Linq.Expressions;
using Data.Models;
using Data.Models.Auth;
using Data.Services.Generic;

namespace Data.Services.Main;

public class UserService(IGenericCrudPaginationService<User> service): IUserService
{
    public async Task<IReadOnlyCollection<User>> GetAllAsync(Expression<Func<User, bool>>? filter) =>
        await service.GetAllAsync(filter);

    public async Task<User?> GetByIdAsync(Guid id) =>
        await service.GetByIdAsync(id);

    public async Task AddAsync(User entity)
    {
        entity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(entity.Password);
        await service.AddAsync(entity);
    }

    public async Task UpdateAsync(User entity)
    {
        entity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(entity.Password);
        await service.UpdateAsync(entity);
    }

    public async Task<PaginationModel<User>> GetAllPagesAsync(int page, int pageSize, Expression<Func<User, bool>>? filter = null) =>
        await service.GetAllPagesAsync(page, pageSize, filter);
}