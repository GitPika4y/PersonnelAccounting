namespace WPF_Desktop.Utils;

public class RememberMeData(string login)
{
    private static DateTime CreateExpireDate() => DateTime.Now.AddSeconds(10);

    public string Login { get; } = login;
    public DateTime ExpireDate { get; set; } = CreateExpireDate();

    public bool IsLogged => ExpireDate > DateTime.Now;

    public void Refresh()
    {
        ExpireDate = CreateExpireDate();
    }
}