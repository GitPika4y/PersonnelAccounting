using System.Linq.Expressions;
using Data.Context;
using Data.Models.Auth;
using Data.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Main;

public class AuthService(AppDbContext context): IAuthService
{
    public async Task<User?> AuthenticateAsync(string login, string password)
    {
        var user = await IdentifyUserAsync(login);

        if (user is null) return null;

        var isPasswordCorrect = BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);

        return isPasswordCorrect ? user : null;
    }

    private async Task<User?> IdentifyUserAsync(string login) =>
        await context.Users.FirstOrDefaultAsync(u => u.Login == login);
}