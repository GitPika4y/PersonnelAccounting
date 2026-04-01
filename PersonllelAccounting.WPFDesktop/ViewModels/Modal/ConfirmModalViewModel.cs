using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;

namespace WPF_Desktop.ViewModels.Modal;

public partial class ConfirmModalViewModel(
    string title,
    string content,
    string? confirmButtonText = null,
    string? denyButtonText = null
    ): ViewModelBase
{
    public string Title { get; } = title;
    public string Content { get; } = content;
    public string ConfirmButtonText { get; } =  confirmButtonText ?? "Ок";
    public string DenyButtonText { get; } = denyButtonText ?? "Закрыть";

    [RelayCommand]
    private void Close() => DialogHost.CloseDialogCommand.Execute(true, null);
}