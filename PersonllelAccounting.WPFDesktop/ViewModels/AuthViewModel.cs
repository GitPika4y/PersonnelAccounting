using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Auth;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.ViewModels.Admin;
using WPF_Desktop.ViewModels.User;

namespace WPF_Desktop.ViewModels;

public partial class AuthViewModel(
    IAuthUseCase useCase,
    INavigationService navigationService,
    ISessionService sessionService
    )
    : ViewModelBase
{
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

    public PackIconKind TogglePasswordVisibilityContent => IsPasswordVisible ? PackIconKind.Eye : PackIconKind.EyeClosed;
    public event Action PasswordVisibilityToggled;

    private bool CanSignIn() => CheckProperties();

    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {
        var resource = await useCase.SignInAsync(Login, Password);
        switch(resource)
        {
            case { IsSuccess: true, Data: not null }:
                var user = resource.Data;
                sessionService.SetUser(user);
                switch (user.Role)
                {
                    case UserRole.Admin:
                        navigationService.Navigate<AdminViewModel>();
                        break;
                    case UserRole.User:
                        navigationService.Navigate<UserViewModel>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;

            case { IsSuccess: true, Data: null }:
                ShowMessage("Вход не выполнен", "Неверные Логин или Пароль");
                break;

            case { IsSuccess: false, ExceptionMessage: not null }:
                ShowMessage("Ошибка", resource.ExceptionMessage);
                break;
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
        PasswordVisibilityToggled?.Invoke();
    }
}