using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Auth;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Services;
using WPF_Desktop.ViewModels.Admin;

namespace WPF_Desktop.ViewModels;

public partial class SideBarViewModel: ViewModelBase
{
    private readonly Dictionary<UserRole, List<SideBarItem>> _sideBarItems = new()
    {
        {
            UserRole.Admin ,
            [
                new SideBarItem(PackIconKind.AccountBoxMultipleOutline, "Пользователи", typeof(AdminUserViewModel)),
                new SideBarItem(PackIconKind.AccountGroupOutline, "Отделы", typeof(AdminDepartmentViewModel)),
                new SideBarItem(PackIconKind.BadgeAccountHorizontalOutline, "Должности", typeof(AdminPositionViewModel))
            ]
        },
        {
            UserRole.User ,
            [

            ]
        }
    };

    public ObservableCollection<SideBarItem> SideBarItems { get; } = [];

    public NavigationService NavigationService { get; }

    public Data.Models.Auth.User CurrentUser { get; }

    public SideBarViewModel(
        IServiceProvider services,
        ISessionService sessionService)
    {
        CurrentUser = sessionService.GetCurrentUser();
        NavigationService = new NavigationService(services);

        var userRole = CurrentUser.Role;
        UpdateObservableCollection(SideBarItems, _sideBarItems[userRole]);
    }

    [RelayCommand]
    private void Navigate(Type viewModelType) => NavigationService.Navigate(viewModelType);
}