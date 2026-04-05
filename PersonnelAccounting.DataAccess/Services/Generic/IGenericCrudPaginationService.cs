using System.Linq.Expressions;
using Data.Models;

namespace Data.Services.Generic;

public interface IGenericCrudPaginationService<T>:
    IGenericCrudService<T>
    where T : EntityModel
{
    Task<PaginationModel<T>> GetAllPagesAsync(Expression<Func<T, bool>>? filter = null,
        int page = 1,
        int pageSize = 10);
}