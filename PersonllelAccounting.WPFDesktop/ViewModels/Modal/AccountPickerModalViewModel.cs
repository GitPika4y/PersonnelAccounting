using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Modal;

public partial class AccountPickerModalViewModel(List<RememberMeData> accounts): ViewModelBase
{
    public ObservableCollection<RememberMeData> Accounts { get; } = new(accounts);

    private void CloseDialog(RememberMeData account, bool isDelete = false)
    {
        var rememberMeItem = new RememberMeItem(account, isDelete);
        DialogHost.CloseDialogCommand.Execute(rememberMeItem, null);
    }

    [RelayCommand]
    private void LogIn(RememberMeData account) => CloseDialog(account);

    [RelayCommand]
    private void Delete(RememberMeData account) => CloseDialog(account, true);
}