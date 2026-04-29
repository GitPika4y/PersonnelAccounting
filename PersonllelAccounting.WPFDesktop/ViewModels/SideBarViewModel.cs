using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Auth;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Services;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Admin;
using WPF_Desktop.ViewModels.User;

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
                new SideBarItem(PackIconKind.BadgeAccountHorizontalOutline, "Должности", typeof(AdminPositionViewModel)),
                new SideBarItem(PackIconKind.ChartArc, "Статистика", typeof(StatisticsViewModel)),
            ]
        },
        {
            UserRole.User ,
            [
                new SideBarItem(PackIconKind.AccountTie, "Сотрудники", typeof(UserEmployeeViewModel)),
                new SideBarItem(PackIconKind.FileSign, "Приказы", typeof(UserOrderViewModel)),
                new SideBarItem(PackIconKind.ChartArc, "Статистика", typeof(StatisticsViewModel)),
            ]
        }
    };

    public ObservableCollection<SideBarItem> SideBarItems { get; } = [];

    public NavigationService NavigationService { get; }

    public Data.Models.Auth.User CurrentUser { get; }

    public SideBarViewModel(
        ISessionService sessionService,
        NavigationRegistry navigationRegistry)
    {
        CurrentUser = sessionService.GetCurrentUser();
        NavigationService = navigationRegistry.Get(NavigationRegion.MainView);

        var userRole = CurrentUser.Role;
        UpdateObservableCollection(SideBarItems, _sideBarItems[userRole]);
    }

    [RelayCommand]
    private void Navigate(Type viewModelType) => NavigationService.Navigate(viewModelType);
}