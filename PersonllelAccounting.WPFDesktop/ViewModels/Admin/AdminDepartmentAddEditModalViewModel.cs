using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminDepartmentAddEditModalViewModel: ViewModelBase
{
    [ObservableProperty] private string _modalTitle = "Добавить новый отдел";
    [ObservableProperty] private string _modalBtnText = "Добавить";

    [ObservableProperty]
    [Required(ErrorMessage = "Обязательное поле")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _title = string.Empty;

    private readonly Guid? _id;

    public AdminDepartmentAddEditModalViewModel(Department? department = null)
    {
        if (department == null)
            return;

        ModalTitle = "Редактировать отдел";
        ModalBtnText = "Редактировать";
        Title = department.Title;
        _id = department.Id;
    }

    private bool CanSave() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        Resource<Department> resource;

        try
        {
            var department = new Department
            {
                Title = Title
            };

            if (_id is not null)
                department.Id = _id.Value;

            resource = Resource<Department>.Success(department);
        }
        catch (Exception e)
        {
            resource = Resource<Department>.Failure($"Ошибка при {(_id is null ? "Добавлении" : "Обновлении")} отдела", e);
        }

        DialogHost.CloseDialogCommand.Execute(resource, null);
    }
}