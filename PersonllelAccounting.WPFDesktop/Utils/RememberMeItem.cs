namespace WPF_Desktop.Utils;

public class RememberMeItem(RememberMeData data, bool readyToDelete = false)
{
    public RememberMeData Data { get; } = data;
    public bool ReadyToDelete { get; } = readyToDelete;
}