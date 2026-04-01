using Data.Models.Auth;
using Data.Services.Generic;

namespace Data.Services.Main;

public interface IAuthService
{
    Task<User?> AuthenticateUserAsync(string login, string password);
    Task<User?> IdentifyUserAsync(string login);
}