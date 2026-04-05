using Data.Context;
using Data.Services.Generic;
using Data.Services.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions;

public static class DiExtensions
{
    private static void AddDatabase(this IServiceCollection services)
    {
        var connectionString = DotEnvExtensions.GetConnectionString();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        });
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddSingleton(typeof(IGenericCrudService<>), typeof(GenericCrudService<>));
        services.AddSingleton(typeof(IGenericCrudPaginationService<>), typeof(GenericCrudPaginationService<>));
        services.AddSingleton<DbInitializer>();
    }

    public static void ProvideDataAccessLibrary(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddServices();
    }
}