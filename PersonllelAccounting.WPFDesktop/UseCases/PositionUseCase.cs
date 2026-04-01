using Data.Models.Main;
using Data.Services.Generic;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class PositionUseCase(IGenericCrudService<Position> service): UseCaseBase, IPositionUseCase
{
    public async Task<Resource<IReadOnlyCollection<Position>>> GetAllAsync() =>
        await SafeCallAsync(service.GetAllAsync);

    public async Task<Resource> AddAsync(Position position) =>
        await SafeCallAsync(() => service.AddAsync(position));

    public async Task<Resource> UpdateAsync(Position position) =>
        await SafeCallAsync(() => service.UpdateAsync(position));
}