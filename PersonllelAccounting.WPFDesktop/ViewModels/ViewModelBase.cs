using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Modal;

namespace WPF_Desktop.ViewModels;

public class ViewModelBase: ObservableValidator
{
    protected void ShowMessage(string title, string message)
    {
        ShowDialog(new MessageModalViewModel(title, message));
    }

    protected async Task<object?> ShowDialog(ViewModelBase vm)
    {
        return await DialogHost.Show(vm);
    }

    protected void HandleResource<T>(Resource<T> resource, Action<T> onSuccess, Action? onError = null, bool showMessageOnError = true)
    {
        switch (resource)
        {
            case {IsSuccess: true, Data: not null}:
                onSuccess(resource.Data);
                break;

            case {IsSuccess: false}:
                if (showMessageOnError)
                    ShowMessage("Ошибка", resource.ExceptionMessage ?? "Неизвестная ошибка");

                onError?.Invoke();
                break;
        }
    }

    protected void HandleResourceMessage<T>(Resource<T> resource, string successMessage, string successTitle = "Успешно",
        string errorTitle = "Ошибка")
    {
        switch (resource.IsSuccess)
        {
            case true:
                ShowMessage(successTitle, successMessage);
                break;
            case false:
                ShowMessage(errorTitle, resource.ExceptionMessage ?? "Неизвестная ошибка");
                break;
        }
    }

    protected void HandleResourceMessage(Resource resource, string successMessage, string successTitle = "Успешно",
        string errorTitle = "Ошибка")
    {
        switch (resource.IsSuccess)
        {
            case true:
                ShowMessage(successTitle, successMessage);
                break;
            case false:
                ShowMessage(errorTitle, resource.ExceptionMessage ?? "Неизвестная ошибка");
                break;
        }
    }

    protected void UpdateObservableCollection<T>(ObservableCollection<T> collection, ICollection<T> source)
    {
        collection.Clear();
        foreach (var item in source)
            collection.Add(item);
    }

    protected bool CheckProperties()
    {
        ValidateAllProperties();
        return !HasErrors;
    }
}