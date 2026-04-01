using System.Linq.Expressions;
using Data.Models.Auth;
using Data.Services.Generic;

namespace Data.Services.Main;

public class UserService(IGenericCrudService<User> service): IUserService
{
    public async Task<IReadOnlyCollection<User>> GetAllAsync() =>
        await service.GetAllAsync();

    public async Task<IReadOnlyCollection<User>> GetAllAsync(Expression<Func<User, bool>> filter) =>
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
        var entityToUpdate = await GetByIdAsync(entity.Id) ?? throw new NullReferenceException("Cannot find entity to update it");

        if (!BCrypt.Net.BCrypt.EnhancedVerify(entity.Password, entityToUpdate.Password))
            entity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(entity.Password);

        await service.UpdateAsync(entity);
    }
}