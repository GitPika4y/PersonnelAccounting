using System.Linq.Expressions;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Generic;

public class GenericCrudPaginationService<T>(
    AppDbContext context,
    IGenericCrudService<T> service) : IGenericCrudPaginationService<T>
    where T : EntityModel
{
    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter) =>
        await service.GetAllAsync(filter);

    public async Task<T?> GetByIdAsync(Guid id) =>
        await service.GetByIdAsync(id);

    public async Task AddAsync(T entity) =>
        await service.AddAsync(entity);

    public async Task UpdateAsync(T entity) =>
        await service.UpdateAsync(entity);

    public async Task<PaginationModel<T>> GetAllPagesAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null)
    {
        var query = context.Set<T>().AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        var count = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginationModel<T>
        {
            Count = count,
            Page = page,
            PageSize = pageSize,
            Items = items,
        };
    }
}