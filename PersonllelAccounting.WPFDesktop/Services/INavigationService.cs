using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Services;

public interface INavigationService
{
    void Navigate<TViewModel>() where TViewModel : ViewModelBase;
    void Navigate(Type viewModelType);
}