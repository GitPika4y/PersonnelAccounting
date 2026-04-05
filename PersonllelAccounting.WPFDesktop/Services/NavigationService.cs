using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Services;

public partial class NavigationService(IServiceProvider services): ObservableObject, INavigationService
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;

    public void ClearHistory()
    {
        CurrentViewModel = null;
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModelBase
    {
        CurrentViewModel = services.GetRequiredService<TViewModel>();
    }

    public void Navigate<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : ViewModelBase, INavigationAware<TParameter>
        where TParameter : class
    {
        var vm = services.GetRequiredService<TViewModel>();

        if (vm is INavigationAware<TParameter> navigationAware)
        {
            navigationAware.OnNavigatedTo(parameter);
        }

        CurrentViewModel = vm;
    }

    public void Navigate(Type viewModelType)
    {
        var vm = services.GetRequiredService(viewModelType) as ViewModelBase
            ?? throw new Exception($"{viewModelType} is not ViewModelBase or not registered");

        CurrentViewModel = vm;
    }
}