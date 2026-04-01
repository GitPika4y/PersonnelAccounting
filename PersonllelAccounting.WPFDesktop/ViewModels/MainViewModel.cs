using WPF_Desktop.Services;
using WPF_Desktop.ViewModels.Admin;

namespace WPF_Desktop.ViewModels;

internal partial class MainViewModel: ViewModelBase
{
    public SideBarViewModel SideBarViewModel { get; }

    public MainViewModel(SideBarViewModel sideBarViewModel, IWindowService windowService)
    {
        SideBarViewModel = sideBarViewModel;
        windowService.SetMinSize(1300, 900);
    }
}