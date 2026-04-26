using Data.Context;
using Data.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

public class DbInitializer(AppDbContext context)
{
    public async Task Initialize()
    {
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync()) return;

        await context.Users.AddAsync(new User
        {
            Login = "admin",
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin123"),
            Role = UserRole.Admin
        });

        await context.SaveChangesAsync();
    }
}