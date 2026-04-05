using CommunityToolkit.Mvvm.Input;
using WPF_Desktop.Services;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels;

public partial class SettingsViewModel(
    NavigationRegistry navigationRegistry
    ): ViewModelBase
{

    [RelayCommand]
    private void LogOut()
    {
        navigationRegistry.Navigate<AuthViewModel>(NavigationRegion.Window);
    }
}