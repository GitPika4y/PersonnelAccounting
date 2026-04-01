using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminPositionAddEditModalViewModel: ViewModelBase
{
    [ObservableProperty] private string _modalTitle = "Новая должность";
    [ObservableProperty] private string _modalBtnText = "Добавить";


    [ObservableProperty]
    [Required(ErrorMessage = "Обязательное поле")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _title = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Обязательное поле")]
    [Range(minimum: 27_000.0, maximum: 1_000_000.0, ErrorMessage = "Заработная плата должа быть в районе [27 000; 1 000 000]")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private decimal _salary;

    private readonly Guid? _id;

    public AdminPositionAddEditModalViewModel(Position? position = null)
    {
        if (position is null)
            return;

        Title = position.Title;
        Salary = position.Salary;
        _id = position.Id;

        ModalTitle = "Редактировать должность";
        ModalBtnText = "Редактировать";
    }

    private bool CanSave() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        Resource<Position> resource;

        try
        {
            var position = new Position
            {
                Title = Title,
                Salary = Salary
            };

            if (_id is not null)
                position.Id = _id.Value;

            resource = Resource<Position>.Success(position);
        }
        catch (Exception e)
        {
            resource = Resource<Position>.Failure($"Ошибка при {(_id is null ? "Создании" : "Обновлении")}", e);
        }

        DialogHost.CloseDialogCommand.Execute(resource, null);
    }
}