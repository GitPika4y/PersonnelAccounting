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
    private IServiceProvider _serviceProvider;
    protected override async void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        try
        {
            services.ProvideDataAccessLibrary();
            services.ProvideDependencies();
            _serviceProvider = services.BuildServiceProvider();

            var dbInitializer = _serviceProvider.GetRequiredService<DbInitializer>();
            await dbInitializer.Initialize();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Ошибка: {exception.Message}");
            Shutdown();
            return;
        }

        var navigationRegistry = _serviceProvider.GetRequiredService<NavigationRegistry>();
        var navigationService = navigationRegistry.Get(NavigationRegion.Window);
        var window = _serviceProvider.GetRequiredService<MainWindow>();

        navigationService.Navigate<AuthViewModel>();

        window.DataContext = navigationService;

        window.Show();
    }
}