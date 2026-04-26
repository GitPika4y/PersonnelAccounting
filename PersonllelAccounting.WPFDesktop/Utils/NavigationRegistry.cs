using WPF_Desktop.Services;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Utils;

public class NavigationRegistry
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<NavigationRegion, NavigationService> _map = [];

    public NavigationRegistry(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var regions = Enum.GetValues<NavigationRegion>();
        foreach (var region in regions)
        {
            _map[region] = new NavigationService(serviceProvider);
        }
    }

    public NavigationService Get(NavigationRegion region) => _map[region];

    public void Navigate<TViewModel>(NavigationRegion region)
        where TViewModel : ViewModelBase
    {
        var service = Get(region);
        service.Navigate<TViewModel>();
    }

    public NavigationService RefreshAndGet(NavigationRegion region)
    {
        var service = Get(region);
        service.ClearHistory();
        return service;
    }

    public void Navigate<TViewModel, TParameter>(NavigationRegion region, TParameter parameter)
        where TViewModel : ViewModelBase, INavigationAware<TParameter>
        where TParameter : class
    {
        var service = Get(region);
        service.Navigate<TViewModel, TParameter>(parameter);
    }
}