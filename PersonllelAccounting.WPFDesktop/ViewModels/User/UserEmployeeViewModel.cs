using System.Collections.ObjectModel;
using System.Linq.Expressions;
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

    [ObservableProperty] private string _nameFilter = string.Empty;

    public IEnumerable<EmployeeStatus> EmployeeStatuses { get; } = Enum.GetValues<EmployeeStatus>();
    [ObservableProperty] private EmployeeStatus _selectedEmployeeStatus;

    partial void OnNameFilterChanged(string value) => _ = UpdatePaginationCollection();
    partial void OnSelectedEmployeeStatusChanged(EmployeeStatus value) => _ = UpdatePaginationCollection();

    public UserEmployeeViewModel(IEmployeeUseCase useCase, NavigationRegistry navigationRegistry)
    {
        _useCase = useCase;
        _navigationService = navigationRegistry.Get(NavigationRegion.MainView);
        _ = UpdatePaginationCollection();
    }

    public async Task InitializeAsync()
    {
    }

    protected override async Task UpdatePaginationCollection()
    {
        await Task.Delay(300);

        Expression<Func<Employee, bool>> dbFilter = (e) =>
            e.FirstName.Contains(NameFilter)
            || e.LastName.Contains(NameFilter)
            || e.MiddleName.Contains(NameFilter);

        var resource = await _useCase.GetAllPagesAsync(SelectedPage, SelectedPageSize, dbFilter);
        await HandleResource(
            resource,
            pagination =>
            {
                var filteredItems = pagination.Items
                    .Where(e => e.Status == SelectedEmployeeStatus)
                    .ToList();

                UpdateObservableCollection(Employees, filteredItems);
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
                await UpdatePaginationCollection();
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
                await UpdatePaginationCollection();
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