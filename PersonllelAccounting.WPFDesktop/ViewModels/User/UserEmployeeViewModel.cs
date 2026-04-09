using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models;
using Data.Models.Main;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class UserEmployeeViewModel: ViewModelPagination<Employee>
{
    private readonly IEmployeeUseCase _useCase;
    private readonly NavigationService _navigationService;

    public ObservableCollection<Employee> Employees { get; } = [];
    [ObservableProperty] private Employee? _selectedEmployee;

    public UserEmployeeViewModel(IEmployeeUseCase useCase, NavigationRegistry navigationRegistry)
    {
        _useCase = useCase;
        _navigationService = navigationRegistry.Get(NavigationRegion.MainView);
        _ = UpdateCollection();
    }

    public async Task InitializeAsync()
    {
    }

    protected override async Task UpdateCollection()
    {
        var resource = await _useCase.GetAllAsync(SelectedPage, SelectedPageSize);
        await HandleResource(
            resource,
            pagination =>
            {
                UpdateObservableCollection(Employees, pagination.Items);
                Pagination = pagination;
            } );
    }

    [RelayCommand]
    private async Task AddEmployee()
    {
        var dialogResult = await ShowDialog(new EmployeeAddEditModalViewModel());
        switch (dialogResult)
        {
            case Resource<Employee> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.AddAsync(successResource.Data);
                await HandleResourceMessage(resource, "Добавление сотрудника успешно");
                await UpdateCollection();
                break;

            case Resource<Employee> { IsSuccess: false, ExceptionMessage: not null} failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }

    [RelayCommand]
    private async Task EditEmployee()
    {
        if (SelectedEmployee is null)
        {
            await ShowMessage("Редактирование невозможно", "Выберите сотрудника чтобы его отредактировать");
            return;
        }

        var dialogResult = await ShowDialog(new EmployeeAddEditModalViewModel(SelectedEmployee));
        switch (dialogResult)
        {
            case Resource<Employee> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.UpdateAsync(successResource.Data);
                await HandleResourceMessage(resource, "Редактирование сотрудника успешно");
                await UpdateCollection();
                break;

            case Resource<Employee> {IsSuccess: false, ExceptionMessage: not null} failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }

    [RelayCommand]
    private void OpenDetail(Employee employee)
    {
        _navigationService.Navigate<EmployeeDetailViewModel, Employee>(employee);
    }
}