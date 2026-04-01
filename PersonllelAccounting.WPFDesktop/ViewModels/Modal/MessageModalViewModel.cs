using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF_Desktop.ViewModels.Modal;

public partial class MessageModalViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _message;

    public MessageModalViewModel(string title, string message)
    {
        Title = title;
        Message = message;
    }
}