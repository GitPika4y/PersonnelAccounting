using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminDepartmentViewModel: ViewModelBase
{
    private readonly IDepartmentUseCase _useCase;

    public ObservableCollection<Department> Departments { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditDepartmentCommand))]
    private Department? _selectedDepartment;

    private bool CanEdit() => SelectedDepartment is not null;

    public AdminDepartmentViewModel(IDepartmentUseCase useCase)
    {
        _useCase = useCase;
        _ = UpdateDepartments();
    }

    private async Task UpdateDepartments()
    {
        var resource = await _useCase.GetAllAsync();
        await HandleResource(
            resource,
            departments => UpdateObservableCollection(Departments, departments.ToList())
            );
    }

    [RelayCommand]
    private async Task AddDepartment()
    {
        var dialogResult = await ShowDialog(new AdminDepartmentAddEditModalViewModel());
        switch (dialogResult)
        {
            case Resource<Department> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.AddAsync(successResource.Data);
                await HandleResourceMessage(resource, "Отдел успешно добавлен");
                await UpdateDepartments();
                break;

            case Resource<Department> {IsSuccess: false, ExceptionMessage: not null} failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private async Task EditDepartment()
    {
        if (SelectedDepartment is null)
        {
            await ShowMessage("Выберите отдел", "Не выбран отдел для редактирования");
            return;
        }

        var dialogResult = await ShowDialog(new AdminDepartmentAddEditModalViewModel(SelectedDepartment));
        switch (dialogResult)
        {
            case Resource<Department> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.UpdateAsync(successResource.Data);
                await HandleResourceMessage(resource, "Редактирование прошло успешно");
                await UpdateDepartments();
                break;

            case Resource<Department> {IsSuccess: false, ExceptionMessage: not null} failedResource:
                await HandleResourceMessage(failedResource, "");
                break;
        }
    }
}