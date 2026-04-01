using System.Windows;
using System.Windows.Controls;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Views;

public partial class AuthView : UserControl
{
    public AuthView()
    {
        InitializeComponent();

    }

    private void HiddenPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is AuthViewModel vm)
            vm.Password = HiddenPasswordBox.Password;
    }

    private async void AuthView_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is AuthViewModel vm)
        {
            vm.PasswordVisibilityToggled += () => HiddenPasswordBox.Password = vm.Password;
            await vm.InitializeAsync();
        }
    }
}