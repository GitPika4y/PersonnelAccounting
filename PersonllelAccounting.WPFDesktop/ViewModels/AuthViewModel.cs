using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.IdentityModel.Tokens;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Admin;

namespace WPF_Desktop.ViewModels;

public partial class AuthViewModel(
    IAuthUseCase useCase,
    NavigationRegistry navigationRegistry,
    ISessionService sessionService,
    IRememberMeService rememberMeService)
    : ViewModelBase
{
    private bool _isLogged;
    private NavigationService NavigationService => navigationRegistry.Get(NavigationRegion.Window);

    [ObservableProperty]
    [Required(ErrorMessage = "Обязательное поле")]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _login = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Обязательное поле")]
    [MinLength(8, ErrorMessage = "Длина должна быть минимум 8 символов")]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TogglePasswordVisibilityContent))]
    private bool _isPasswordVisible;

    [ObservableProperty] private bool _isRememberMeChecked;

    public PackIconKind TogglePasswordVisibilityContent => IsPasswordVisible ? PackIconKind.Eye : PackIconKind.EyeClosed;
    public event Action? PasswordVisibilityToggled;

    public async Task InitializeAsync()
    {
        await Authenticate();
    }

    private bool CanSignIn() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {
        var resource = _isLogged
            ? await useCase.IdentifyUserAsync(Login)
            : await useCase.SignInAsync(Login, Password);
        switch(resource)
        {
            case { IsSuccess: true, Data: not null }:
                var user = resource.Data;
                sessionService.SetUser(user);

                if (IsRememberMeChecked)
                    rememberMeService.Save(new RememberMeData(Login));

                NavigationService.Navigate<MainViewModel>();
                break;

            case { IsSuccess: true, Data: null }:
                await ShowMessage("Вход не выполнен", "Неверные Логин или Пароль");
                break;

            case { IsSuccess: false, ExceptionMessage: not null }:
                await ShowMessage("Ошибка", resource.ExceptionMessage);
                break;
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
        PasswordVisibilityToggled?.Invoke();
    }

    [RelayCommand]
    private async Task Authenticate()
    {
        var data = rememberMeService.Load();
        if (data is null || data.Count == 0)
        {
            await ShowMessage("Пусто", "Последних входов не обнаружено");
            return;
        }

        var selected = await ShowDialog(new Modal.AccountPickerModalViewModel(data));

        if (selected is not RememberMeItem rememberMeItem)
            return;

        if (rememberMeItem.ReadyToDelete)
        {
            rememberMeService.RemoveItem(rememberMeItem.Data);
            return;
        }

        Login = rememberMeItem.Data.Login;

        if (rememberMeItem.Data.IsLogged)
        {
            _isLogged = true;
            await SignIn();
        }
    }
}