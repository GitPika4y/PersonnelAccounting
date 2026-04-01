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
        return await SafeCallAsync(() => service.AuthenticateAsync(username, password));
    }
}