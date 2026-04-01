using Data.Models.Auth;
using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public interface IAuthUseCase
{
    Task<Resource<User?>> SignInAsync(string username, string password);
}