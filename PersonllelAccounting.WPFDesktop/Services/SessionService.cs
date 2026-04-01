using Data.Models.Auth;

namespace WPF_Desktop.Services;

public class SessionService: ISessionService
{
    private User _user = null!;

    public void SetUser(User user) => _user = user;

    public User GetCurrentUser() => _user;
}