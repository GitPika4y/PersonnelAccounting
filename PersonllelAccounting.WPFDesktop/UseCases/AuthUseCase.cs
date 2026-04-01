using Data.Models.Auth;
using Data.Services.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public class AuthUseCase(
    IAuthService service
    ): UseCaseBase, IAuthUseCase
{
    public async Task<Resource<User?>> SignInAsync(string username, string password)
    {
        return await SafeCallAsync(() => service.AuthenticateUserAsync(username, password));
    }

    public async Task<Resource<User?>> IdentifyUserAsync(string username)
    {
        return await SafeCallAsync(() => service.IdentifyUserAsync(username));
    }
}