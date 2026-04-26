namespace WPF_Desktop.Utils;

public class RememberMeData(string login)
{
    private static DateTime CreateExpireDate() => DateTime.Now.AddDays(1);

    public string Login { get; } = login;
    public DateTime ExpireDate { get; set; } = CreateExpireDate();

    public bool IsLogged => ExpireDate > DateTime.Now;

    public void Refresh()
    {
        ExpireDate = CreateExpireDate();
    }
}