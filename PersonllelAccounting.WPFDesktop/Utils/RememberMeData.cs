namespace WPF_Desktop.Utils;

public class RememberMeData(string login)
{
    public string Login { get; } = login;
    public DateTime ExpireDate { get; private set; } = DateTime.Now.AddDays(5);

    public bool IsLogged => ExpireDate > DateTime.Now;

    public void Refresh()
    {
        ExpireDate = DateTime.Now.AddDays(5);
    }
}