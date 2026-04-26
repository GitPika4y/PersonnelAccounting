using WPF_Desktop.Services;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Admin;

namespace WPF_Desktop.ViewModels;

internal partial class MainViewModel: ViewModelBase
{
    public SideBarViewModel SideBarViewModel { get; }
    public NavigationService NavigationService { get; }

    public MainViewModel(
        SideBarViewModel sideBarViewModel,
        IWindowService windowService,
        NavigationRegistry navigationRegistry)
    {
        SideBarViewModel = sideBarViewModel;
        NavigationService = navigationRegistry.RefreshAndGet(NavigationRegion.MainView);
        windowService.SetMinSize(1300, 900);
    }
}