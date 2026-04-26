using System.Windows;
using Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using WPF_Desktop.Services;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        services.ProvideDataAccessLibrary();
        services.ProvideDependencies();
        var serviceProvider = services.BuildServiceProvider();

        var dbInitializer = serviceProvider.GetRequiredService<DbInitializer>();
        await dbInitializer.Initialize();

        var navigationRegistry = serviceProvider.GetRequiredService<NavigationRegistry>();
        var navigationService = navigationRegistry.Get(NavigationRegion.Window);
        var window = serviceProvider.GetRequiredService<MainWindow>();

        navigationService.Navigate<AuthViewModel>();

        window.DataContext = navigationService;

        window.Show();
    }
}