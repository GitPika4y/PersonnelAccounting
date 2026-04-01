using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Services;
using WPF_Desktop.ViewModels.Admin;

namespace WPF_Desktop.ViewModels;

public partial class SideBarViewModel(
    IServiceProvider services,
    ISessionService sessionService
    ): ViewModelBase
{
    public NavigationService NavigationService { get; } = new(services);
    public Data.Models.Auth.User CurrentUser => sessionService.GetCurrentUser();

    public IReadOnlyCollection<SideBarItem> SideBarItems =>
    [
        new(PackIconKind.AccountBoxMultipleOutline, "Пользователи", typeof(AdminUserViewModel),  NavigateCommand),
        new(PackIconKind.AccountGroupOutline, "Отделы", typeof(AdminDepartmentViewModel), NavigateCommand),
        new(PackIconKind.BadgeAccountHorizontalOutline, "Должности", typeof(AdminPositionViewModel), NavigateCommand),
    ];

    [RelayCommand]
    private void Navigate(Type viewModelType) => NavigationService.Navigate(viewModelType);

    [RelayCommand]
    private void OpenSettings() => NavigationService.Navigate<SettingsViewModel>();
}