using System.Linq.Expressions;
using Data.Models;

namespace Data.Services.Generic;

public interface IGenericCrudPaginationService<T>
    : IGenericCrudService<T>
    where T : EntityModel
{
    Task<PaginationModel<T>> GetAllPagesAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null);
}