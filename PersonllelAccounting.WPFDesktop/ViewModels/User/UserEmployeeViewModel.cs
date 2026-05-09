using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models;
using Data.Models.Main;
using Microsoft.Win32;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Modal;

namespace WPF_Desktop.ViewModels.User;

public partial class UserEmployeeViewModel: ViewModelPagination<Employee>
{
    private readonly IEmployeeUseCase _useCase;
    private readonly NavigationService _navigationService;
    private readonly IEmployeeReportExportService  _reportExportService;

    public ObservableCollection<Employee> Employees { get; } = [];
    [ObservableProperty] private Employee? _selectedEmployee;

    [ObservableProperty] private bool _useFilters;
    [ObservableProperty] private string _nameFilter = string.Empty;

    public IEnumerable<EmployeeStatus> EmployeeStatuses { get; } = Enum.GetValues<EmployeeStatus>();
    [ObservableProperty] private EmployeeStatus _selectedEmployeeStatus;

    partial void OnNameFilterChanged(string value) => _ = UpdatePaginationCollection();
    partial void OnSelectedEmployeeStatusChanged(EmployeeStatus value) => _ = UpdatePaginationCollection();
    partial void OnUseFiltersChanged(bool value) => _ = UpdatePaginationCollection();

    public UserEmployeeViewModel(IEmployeeUseCase useCase, NavigationRegistry navigationRegistry, IEmployeeReportExportService reportExportService)
    {
        _useCase = useCase;
        _reportExportService = reportExportService;
        _navigationService = navigationRegistry.Get(NavigationRegion.MainView);
        _ = UpdatePaginationCollection();
    }

    protected override async Task UpdatePaginationCollection()
    {
        await Task.Delay(300);

        Expression<Func<Employee, bool>>? dbFilter = null;

        if (UseFilters)
        {
            dbFilter = BuildDbFilter();
        }

        var resource = await _useCase.GetAllAsync(dbFilter);

        await HandleResource(resource, employees =>
        {
            var filtered = employees;

            if (UseFilters)
            {
                filtered = employees
                    .Where(e => e.Status == SelectedEmployeeStatus)
                    .ToList();
            }

            var paged = filtered
                .Skip((SelectedPage - 1) * SelectedPageSize)
                .Take(SelectedPageSize)
                .ToList();

            UpdateObservableCollection(Employees, paged);

            Pagination = new PaginationModel<Employee>
            {
                Count = filtered.Count,
                Items = paged,
                Page = SelectedPage,
                PageSize = SelectedPageSize
            };
        });
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
    private async Task BuildReport()
    {
        EmployeeFilters? filters = null;
        Expression<Func<Employee, bool>>? dbFilter = null;

        if (UseFilters)
        {
            filters = new EmployeeFilters
            {
                Name = NameFilter,
                Status = SelectedEmployeeStatus
            };

            dbFilter = BuildDbFilter();
        }

        var employeesResource = await _useCase.GetAllAsync(dbFilter);
        await HandleResource(employeesResource, async employees =>
        {
            var filtered = employees;
            if (UseFilters)
                filtered = employees.Where(e => e.Status == SelectedEmployeeStatus).ToList();

            var dialog = await ShowDialog(new EmployeeReportExportModalViewModel(filtered, filters));
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
                _reportExportService.BuildReport(filtered, filters, savePath);
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
    private void OpenDetail(Employee employee)
    {
        _navigationService.Navigate<EmployeeDetailViewModel, Employee>(employee);
    }

    private Expression<Func<Employee, bool>> BuildDbFilter()
    {
        return e =>
            e.FirstName.Contains(NameFilter) ||
            e.LastName.Contains(NameFilter) ||
            e.MiddleName.Contains(NameFilter);
    }

    private string? SelectFilePath()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Сохранить отчет",
            Filter = "Excel files (*.xlsx)|*.xlsx",
            DefaultExt = ".xlsx",
            FileName = "Отчет_Сотрудники"
        };

        var result = dialog.ShowDialog();
        return result == true
            ? dialog.FileName
            : null;
    }
}