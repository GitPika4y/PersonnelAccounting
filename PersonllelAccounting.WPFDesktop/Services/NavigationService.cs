using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Services;

public partial class NavigationService(IServiceProvider services): ObservableObject, INavigationService
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel = null!;

    public void Navigate<TViewModel>() where TViewModel : ViewModelBase
    {
        CurrentViewModel = services.GetRequiredService<TViewModel>();
    }

    public void Navigate(Type viewModelType)
    {
        var vm = services.GetRequiredService(viewModelType) as ViewModelBase
            ?? throw new Exception($"{viewModelType} is not registered");

        CurrentViewModel = vm;
    }
}