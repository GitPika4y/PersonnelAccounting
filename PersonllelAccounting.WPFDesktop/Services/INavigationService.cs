using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Services;

public interface INavigationService
{
    void Navigate<TViewModel>() where TViewModel : ViewModelBase;

    void Navigate<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : ViewModelBase, INavigationAware<TParameter>
        where TParameter : class;
    void Navigate(Type viewModelType);
}