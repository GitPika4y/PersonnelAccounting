using System.Linq.Expressions;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Generic;

public class GenericCrudService<T>(AppDbContext context): IGenericCrudService<T> where T : EntityModel
{
    public async Task<IReadOnlyCollection<T>> GetAllAsync() =>
         await context.Set<T>().AsNoTracking().ToListAsync();

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter) =>
        await context.Set<T>().Where(filter).AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        var existing = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id)
            ?? throw new Exception($"Entity with id {entity.Id} was not found");

        context.Entry(existing).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }
}