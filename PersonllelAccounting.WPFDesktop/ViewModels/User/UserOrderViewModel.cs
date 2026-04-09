using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using Data.Services.Main;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class UserOrderViewModel: ViewModelPagination<Order>
{
    private readonly IEmployeeUseCase _employeeUseCase;
    private readonly IOrderTypeService _orderTypeService;
    private readonly IPositionUseCase _positionUseCase;
    private readonly IDepartmentUseCase _departmentUseCase;
    private readonly IOrderUseCase _orderUseCase;

    public ObservableCollection<Order> Orders { get; } = [];

    public UserOrderViewModel(
        IEmployeeUseCase employeeUseCase,
        IOrderTypeService orderTypeService,
        IPositionUseCase positionUseCase,
        IDepartmentUseCase departmentUseCase,
        IOrderUseCase orderUseCase)
    {
        _employeeUseCase = employeeUseCase;
        _orderTypeService = orderTypeService;
        _positionUseCase = positionUseCase;
        _departmentUseCase = departmentUseCase;
        _orderUseCase = orderUseCase;
        _ = UpdateCollection();
    }

    public async Task InitializeAsync()
    {
    }

    protected override async Task UpdateCollection()
    {
        var resource = await _orderUseCase.GetAllAsync(SelectedPage, SelectedPageSize);
        await HandleResource(
            resource,
            paginationModel =>
            {
                UpdateObservableCollection(Orders, paginationModel.Items);
                Pagination = paginationModel;
            });
    }

    [RelayCommand]
    private async Task AddOrder()
    {
        var positionsResource = await _positionUseCase.GetAllAsync();
        var departmentsResource = await _departmentUseCase.GetAllAsync();

        ICollection<Position> positions = [];
        ICollection<Department> departments = [];

        await HandleResource(
            positionsResource,
            p => positions = p.ToList());
        await HandleResource(
            departmentsResource,
            d => departments = d.ToList());

        var dialogResult = await ShowDialog(new OrderAddModalViewModel(_employeeUseCase, _orderTypeService, positions, departments));

        switch (dialogResult)
        {
            case Resource<Order> { IsSuccess: true, Data: not null } successResource:
                var resource = await _orderUseCase.AddAsync(successResource.Data);
                await HandleResourceMessage(resource, "Добавление приказа успешно");
                await UpdateCollection();
                break;

            case Resource<Order> { IsSuccess: false, ExceptionMessage: not null } failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }
}