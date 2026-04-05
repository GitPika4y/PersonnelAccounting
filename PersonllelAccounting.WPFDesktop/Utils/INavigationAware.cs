namespace WPF_Desktop.Utils;

public interface INavigationAware<T> where T: class
{
    void OnNavigatedTo(T parameter);
}