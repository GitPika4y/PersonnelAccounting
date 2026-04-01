using CommunityToolkit.Mvvm.Input;
using WPF_Desktop.Services;

namespace WPF_Desktop.ViewModels;

public partial class SettingsViewModel(
    INavigationService navigationService
    ): ViewModelBase
{

    [RelayCommand]
    private void LogOut()
    {
        navigationService.Navigate<AuthViewModel>();
    }
}