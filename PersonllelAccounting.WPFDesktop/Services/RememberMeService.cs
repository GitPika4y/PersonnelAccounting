using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WPF_Desktop.Utils;

namespace WPF_Desktop.Services;

public class RememberMeService: IRememberMeService
{
    private const string FilePath = "remembe.dat";

    private void Save(List<RememberMeData> data)
    {
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);

        var encrypted = ProtectedData.Protect(
            bytes,
            null,
            DataProtectionScope.CurrentUser);

        File.WriteAllBytes(FilePath, encrypted);
    }

    public void Save(RememberMeData data)
    {
        var savedData = Load() ?? [];

        var savedUser = savedData.FirstOrDefault(x => x.Login == data.Login);

        if (savedUser is null)
            savedData.Add(data);
        else
            savedUser.Refresh();

        Save(savedData);
    }

    public List<RememberMeData>? Load()
    {
        if (!File.Exists(FilePath))
            return null;

        var encrypted = File.ReadAllBytes(FilePath);
        var decrypted = ProtectedData.Unprotect(
            encrypted,
            null,
            DataProtectionScope.CurrentUser);

        var json = Encoding.UTF8.GetString(decrypted);
        var data = JsonSerializer.Deserialize<List<RememberMeData>>(json);

        return data ?? [];
    }

    public void RemoveItem(RememberMeData item)
    {
        var savedData = Load() ?? [];
        var savedUser = savedData.FirstOrDefault(x => x.Login == item.Login);

        if (savedUser is null)
            return;

        savedData.Remove(savedUser);
        Save(savedData);
    }
}