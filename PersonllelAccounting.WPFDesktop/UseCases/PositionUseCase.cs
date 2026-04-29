using System.Linq.Expressions;
using System.Windows;
using Data.Models;
using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class PositionUseCase(IGenericCrudPaginationService<Position> service): UseCaseBase, IPositionUseCase
{
    public async Task<Resource<PaginationModel<Position>>> GetAllAsync(int page, int pageSize,
        Expression<Func<Position, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllPagesAsync(page, pageSize, filter));

    public async Task<Resource<IReadOnlyCollection<Position>>> GetAllAsync(Expression<Func<Position, bool>>? filter = null) =>
        await SafeCallAsync(() => service.GetAllAsync(filter));

    public async Task<Resource> AddAsync(Position position) =>
        await SafeCallAsync(() => service.AddAsync(position));

    public async Task<Resource> UpdateAsync(Position position) =>
        await SafeCallAsync(() => service.UpdateAsync(position));
}