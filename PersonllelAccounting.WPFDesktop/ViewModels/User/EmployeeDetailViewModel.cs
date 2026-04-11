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
    ): ViewModelBase, INavigationAware<Employee>
{
    [ObservableProperty] private Employee _employee = null!;

    public void OnNavigatedTo(Employee parameter)
    {
        Employee = parameter;
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
}