using System.Collections.ObjectModel;
using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
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

    public IEnumerable<OrderType> OrderTypes { get; } = Enum.GetValues<OrderType>();
    [ObservableProperty] private OrderType _selectedOrderType = OrderType.Hire;
    [ObservableProperty] private string _employeeNameFilter = string.Empty;
    [ObservableProperty] private DateTime? _startDateFilter = null;
    [ObservableProperty] private DateTime? _endDateFilter = null;

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
        _ = UpdatePaginationCollection();
    }

    partial void OnSelectedOrderTypeChanged(OrderType value) => _ = UpdatePaginationCollection();
    partial void OnEmployeeNameFilterChanged(string value) => _ = UpdatePaginationCollection();
    partial void OnStartDateFilterChanged(DateTime? value) => _ = UpdatePaginationCollection();
    partial void OnEndDateFilterChanged(DateTime? value) => _ = UpdatePaginationCollection();

    public async Task InitializeAsync()
    {
    }

    protected override async Task UpdatePaginationCollection()
    {
        await Task.Delay(500);

        Expression<Func<Order, bool>> dbFilter = o =>
        // Тип (опционально)
        (SelectedOrderType == null || o.Type == SelectedOrderType)

        // Дата начала (опционально)
        && (!StartDateFilter.HasValue || o.StartDate >= StartDateFilter.Value)

        // Дата окончания (опционально)
        && (!EndDateFilter.HasValue || (o.EndDate.HasValue && o.EndDate <= EndDateFilter.Value))

        // Поиск по сотруднику
        && (
            string.IsNullOrWhiteSpace(EmployeeNameFilter)
            || (
                o.Employee != null &&
                (
                    o.Employee.FirstName.Contains(EmployeeNameFilter)
                    || o.Employee.LastName.Contains(EmployeeNameFilter)
                    || o.Employee.MiddleName.Contains(EmployeeNameFilter)
                )
            )
        );

        var resource = await _orderUseCase.GetAllAsync(SelectedPage, SelectedPageSize, dbFilter);
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
                await UpdatePaginationCollection();
                break;

            case Resource<Order> { IsSuccess: false, ExceptionMessage: not null } failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }

    [RelayCommand]
    private async Task OpenDetail(Order order)
    {
        await ShowDialog(new OrderDetailModalViewModel(order));
    }
}