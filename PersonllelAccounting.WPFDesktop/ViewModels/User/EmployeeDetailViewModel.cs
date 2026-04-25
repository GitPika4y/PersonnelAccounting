using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using Data.Services.Main;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class EmployeeDetailViewModel(
    IOrderTypeService orderTypeService,
    IPositionUseCase positionUseCase,
    IDepartmentUseCase departmentUseCase,
    IOrderUseCase orderUseCase,
    IEmployeeUseCase employeeUseCase
    ): ViewModelPagination<Order>, INavigationAware<Employee>
{
    public ObservableCollection<Order> Orders { get; } = [];

    [ObservableProperty] private Employee _employee = null!;

    public void OnNavigatedTo(Employee parameter)
    {
        Employee = parameter;
        _ = UpdatePaginationCollection();
    }

    public async Task UpdateCurrentEmployee()
    {
        var employeeId = Employee.Id;
        var resource = await employeeUseCase.GetByIdAsync(employeeId);
        await HandleResource(
            resource,
            updatedEmployee => Employee = updatedEmployee!);
    }

    [RelayCommand]
    private async Task AddOrder()
    {
        var positionsResource = await positionUseCase.GetAllAsync();
        var departmentsResource = await departmentUseCase.GetAllAsync();

        ICollection<Position> positions = [];
        ICollection<Department> departments = [];

        await HandleResource(positionsResource,
            p => positions = p.ToList());
        await HandleResource(departmentsResource,
            d => departments = d.ToList());

        var dialog = await ShowDialog(new OrderAddModalViewModel(Employee, orderTypeService, positions, departments));
        switch (dialog)
        {
            case Resource<Order> { IsSuccess: true, Data: not null} successfulResource:
                var resource = await orderUseCase.AddAsync(successfulResource.Data);
                await HandleResourceMessage(resource, "Приказ успешно добавлен");
                await UpdateCurrentEmployee();
                break;

            case Resource<Order> { IsSuccess: false, ExceptionMessage: not null} failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }

    protected override async Task UpdatePaginationCollection()
    {
        var resource = await orderUseCase.GetAllAsync(SelectedPage, SelectedPageSize, o => o.EmployeeId == Employee.Id);
        await HandleResource(
            resource,
            paginationModel =>
            {
                Pagination = paginationModel;
                UpdateObservableCollection(Orders, paginationModel.Items);
            });
    }

    [RelayCommand]
    private async Task OpenDetailOrder(Order order)
    {
        await ShowDialog(new OrderDetailModalViewModel(order));
    }
}