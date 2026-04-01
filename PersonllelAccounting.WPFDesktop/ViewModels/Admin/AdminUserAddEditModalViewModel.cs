using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Auth;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Extensions;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminUserAddEditModalViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _modalTitle = "Добавить пользователя";

    [ObservableProperty]
    private string _modalBtnText = "Добавить";

    [ObservableProperty]
    [Required(ErrorMessage = "Логин обязателен")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _login = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(length:8, ErrorMessage = "Пароль должен состоять из 8 и более символов")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _password= string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Роль обязательна")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private UserRole _role;

    [ObservableProperty]
    private string? _username = string.Empty;

    public ObservableCollection<UserRole> Roles { get; } = new(Enum.GetValues<UserRole>());

    private readonly Guid? _id;

    public AdminUserAddEditModalViewModel(Data.Models.Auth.User? user = null)
    {
        if (user is null) return;

        Login =  user.Login;
        Username = user.Username;
        Role = Roles.First();
        _id = user.Id;

        ModalTitle = "Редактировать пользователя";
        ModalBtnText = "Сохранить";
    }

    private bool CanSave() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        Resource<Data.Models.Auth.User> resource;
        try
        {
            var user = new Data.Models.Auth.User
            {
                Login = Login,
                Password = Password,
                Role = Role,
                Username = Username
            };

            if (_id is not null)
                user.Id = _id.Value;

            resource = Resource<Data.Models.Auth.User>.Success(user);
        }
        catch (Exception e)
        {
            resource = Resource<Data.Models.Auth.User>.Failure($"Ошибка при {(_id is null ? "Добавлении" : "Обновлении")}", e);
        }

        DialogHost.CloseDialogCommand.Execute(resource, null);
    }
}