using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public interface IRememberMeService
{
    void Save(RememberMeData data);
    List<RememberMeData>? Load();
    void RemoveItem(RememberMeData item);
}