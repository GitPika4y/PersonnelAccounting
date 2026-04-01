using Data.Models.Auth;
using Data.Services.Generic;

namespace Data.Services.Main;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string login, string password);
}