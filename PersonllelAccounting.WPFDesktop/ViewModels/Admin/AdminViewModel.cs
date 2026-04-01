using WPF_Desktop.Services;

namespace WPF_Desktop.ViewModels.Admin;

internal partial class AdminViewModel: ViewModelBase
{
    public AdminSideBarViewModel SideBarViewModel { get; }

    public AdminViewModel(AdminSideBarViewModel sideBarViewModel, IWindowService windowService)
    {
        SideBarViewModel = sideBarViewModel;
        windowService.SetMinSize(1300, 900);
    }
}