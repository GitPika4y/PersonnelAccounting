using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Services;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminSideBarViewModel(IServiceProvider services): ViewModelBase
{
    public NavigationService NavigationService { get; } = new(services);

    [RelayCommand]
    private void Navigate(Type viewModelType)
    {
        NavigationService.Navigate(viewModelType);
    }

    public IReadOnlyCollection<SideBarItem> SideBarItems =>
    [
        new(PackIconKind.AccountBoxMultipleOutline, "Пользователи", typeof(AdminUserViewModel),  NavigateCommand),
        new(PackIconKind.AccountGroupOutline, "Отделы", typeof(AdminDepartmentViewModel), NavigateCommand),
        new(PackIconKind.BadgeAccountHorizontalOutline, "Должности", typeof(AdminPositionViewModel), NavigateCommand),
    ];
}