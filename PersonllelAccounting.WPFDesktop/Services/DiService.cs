using Microsoft.Extensions.DependencyInjection;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels;
using WPF_Desktop.ViewModels.Admin;
using WPF_Desktop.ViewModels.User;

namespace WPF_Desktop.Services;

public static class DiService
{
    public static void ProvideDependencies(this IServiceCollection services)
    {
        // AUTH
        services.AddTransient<AuthViewModel>();

        // MAIN
        services.AddTransient<MainViewModel>();
        services.AddTransient<SideBarViewModel>();
        services.AddTransient<SettingsViewModel>();

        // ADMIN
        services.AddTransient<AdminUserViewModel>();
        services.AddTransient<AdminDepartmentViewModel>();
        services.AddTransient<AdminPositionViewModel>();

        services.AddScoped<IAuthUseCase, AuthUseCase>();
        services.AddScoped<IUserUseCase, UserUseCase>();
        services.AddScoped<IDepartmentUseCase, DepartmentUseCase>();
        services.AddScoped<IPositionUseCase, PositionUseCase>();

        // USER
        services.AddTransient<UserEmployeeViewModel>();
        services.AddTransient<UserOrderViewModel>();
        services.AddTransient<EmployeeDetailViewModel>();

        services.AddScoped<IEmployeeUseCase, EmployeeUseCase>();

        // SINGLETONS
        services.AddSingleton<MainWindow>();
        services.AddSingleton<NavigationRegistry>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IRememberMeService, RememberMeService>();
    }
}