using Data.Models.Auth;

namespace WPF_Desktop.Services;

public interface ISessionService
{
    void SetUser(User user);
    User GetCurrentUser();
}