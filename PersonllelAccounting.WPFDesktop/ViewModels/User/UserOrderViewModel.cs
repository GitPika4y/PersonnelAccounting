using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using Data.Services.Main;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;
using Microsoft.Win32;
using WPF_Desktop.ViewModels.Modal;

namespace WPF_Desktop.ViewModels.User;

public partial class UserOrderViewModel: ViewModelPagination<Order>
{
    private readonly IEmployeeUseCase _employeeUseCase;
    private readonly IOrderTypeService _orderTypeService;
    private readonly IPositionUseCase _positionUseCase;
    private readonly IDepartmentUseCase _departmentUseCase;
    private readonly IOrderUseCase _orderUseCase;
    private readonly IOrderReportExportService _orderReportExportService;

    public ObservableCollection<Order> Orders { get; } = [];

    public IEnumerable<OrderType> OrderTypes { get; } = Enum.GetValues<OrderType>();
    [ObservableProperty] private OrderType _selectedOrderType = OrderType.Hire;
    [ObservableProperty] private string _employeeNameFilter = string.Empty;
    [ObservableProperty] private DateTime? _startDateFilter = null;
    [ObservableProperty] private DateTime? _endDateFilter = null;

    [ObservableProperty] private bool _useFilters;

    public UserOrderViewModel(
        IEmployeeUseCase employeeUseCase,
        IOrderTypeService orderTypeService,
        IPositionUseCase positionUseCase,
        IDepartmentUseCase departmentUseCase,
        IOrderUseCase orderUseCase,
        IOrderReportExportService orderReportExportService)
    {
        _employeeUseCase = employeeUseCase;
        _orderTypeService = orderTypeService;
        _positionUseCase = positionUseCase;
        _departmentUseCase = departmentUseCase;
        _orderUseCase = orderUseCase;
        _orderReportExportService = orderReportExportService;
        _ = UpdatePaginationCollection();
    }

    partial void OnSelectedOrderTypeChanged(OrderType value) => _ = UpdatePaginationCollection();
    partial void OnEmployeeNameFilterChanged(string value) => _ = UpdatePaginationCollection();
    partial void OnStartDateFilterChanged(DateTime? value) => _ = UpdatePaginationCollection();
    partial void OnEndDateFilterChanged(DateTime? value) => _ = UpdatePaginationCollection();
    partial void OnUseFiltersChanged(bool value) => _ = UpdatePaginationCollection();

    public async Task InitializeAsync()
    {
    }

    protected override async Task UpdatePaginationCollection()
    {
        await Task.Delay(500);

        Expression<Func<Order, bool>>? dbFilter = null;

        if (UseFilters)
        {
            dbFilter = BuildOrderExpressionFilter();
        }

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
    private async Task BuildReport()
    {
        OrderFilters? filters = null;
        Expression<Func<Order, bool>>? dbFilter = null;

        if (UseFilters)
        {
            filters = new OrderFilters
            {
                EmployeeName =  EmployeeNameFilter,
                EndDate = EndDateFilter,
                StartDate = StartDateFilter,
                OrderType = SelectedOrderType
            };

            dbFilter = BuildOrderExpressionFilter();
        }

        var ordersResource = await _orderUseCase.GetAllAsync(dbFilter);
        await HandleResource(
            ordersResource, async orders =>
            {
                var dialog = await ShowDialog(new OrderReportExportModalViewModel(orders, filters));
                if (dialog is not true)
                    return;

                var savePath = SelectFilePath();
                if (savePath is null)
                {
                    await ShowMessage("Ошибка", "Место сохранения файла не было выбрано");
                    return;
                }

                try
                {
                    _orderReportExportService.BuildReport(orders, filters, savePath);
                }
                catch (Exception e)
                {
                    await ShowMessage("Ошибка", e.Message);
                    return;
                }

                var open = await ShowDialog(new ConfirmModalViewModel("Успешно", "Отчет был успешно сгенерирован", "Открыть файл", "Ок"));
                if (open is true)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = savePath,
                        UseShellExecute = true
                    });
                }
            });
    }

    [RelayCommand]
    private async Task OpenDetail(Order order)
    {
        await ShowDialog(new OrderDetailModalViewModel(order));
    }

    private Expression<Func<Order, bool>> BuildOrderExpressionFilter()
    {
        return o =>
            (SelectedOrderType == null || o.Type == SelectedOrderType)
            && (!StartDateFilter.HasValue || o.StartDate >= StartDateFilter.Value)
            && (!EndDateFilter.HasValue || (o.EndDate.HasValue && o.EndDate <= EndDateFilter.Value))
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
    }

    private string? SelectFilePath()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Сохранить отчет",
            Filter = "Excel files (*.xlsx)|*.xlsx",
            DefaultExt = ".xlsx",
            FileName = "Отчет_Приказы"
        };

        var result = dialog.ShowDialog();
        return result == true
            ? dialog.FileName
            : null;
    }
}