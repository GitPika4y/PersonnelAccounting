using System.Linq.Expressions;

namespace Data.Services.Generic;

public interface IGenericCrudService<T> where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
}