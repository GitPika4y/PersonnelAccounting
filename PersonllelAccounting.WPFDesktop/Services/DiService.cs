using Microsoft.Extensions.DependencyInjection;
using WPF_Desktop.UseCases;
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

        // ADMIN
        services.AddTransient<AdminViewModel>();
        services.AddTransient<AdminSideBarViewModel>();
        services.AddTransient<AdminUserViewModel>();
        services.AddTransient<AdminDepartmentViewModel>();
        services.AddTransient<AdminPositionViewModel>();

        services.AddScoped<IAuthUseCase, AuthUseCase>();
        services.AddScoped<IUserUseCase, UserUseCase>();
        services.AddScoped<IDepartmentUseCase, DepartmentUseCase>();
        services.AddScoped<IPositionUseCase, PositionUseCase>();

        // USER
        services.AddTransient<UserViewModel>();

        // SINGLETONS
        services.AddSingleton<MainWindow>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IWindowService, WindowService>();
    }
}