using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using WPF_Desktop.Utils;
using WPF_Desktop.ViewModels.Modal;

namespace WPF_Desktop.ViewModels;

public class ViewModelBase: ObservableValidator
{
    protected async Task ShowMessage(string title, string message)
    {
        await ShowDialog(new MessageModalViewModel(title, message));
    }

    protected async Task ShowConfirmDialog(string title, string content, string? confirmButtonText = null, string? denyButtonText = null)
    {
        await ShowDialog(new ConfirmModalViewModel(title, content, confirmButtonText, denyButtonText));
    }

    protected async Task<object?> ShowDialog(ViewModelBase vm)
    {
        return await DialogHost.Show(vm);
    }

    protected async Task HandleResource<T>(Resource<T> resource, Action<T> onSuccess, Action? onError = null, bool showMessageOnError = true)
    {
        switch (resource)
        {
            case {IsSuccess: true, Data: not null}:
                onSuccess(resource.Data);
                break;

            case {IsSuccess: false}:
                if (showMessageOnError)
                    await ShowMessage("Ошибка", resource.ExceptionMessage ?? "Неизвестная ошибка");

                onError?.Invoke();
                break;
        }
    }

    protected async Task HandleResourceMessage<T>(Resource<T> resource, string successMessage, string successTitle = "Успешно",
        string errorTitle = "Ошибка")
    {
        switch (resource.IsSuccess)
        {
            case true:
                await ShowMessage(successTitle, successMessage);
                break;
            case false:
                await ShowMessage(errorTitle, resource.ExceptionMessage ?? "Неизвестная ошибка");
                break;
        }
    }

    protected async Task HandleResourceMessage(Resource resource, string successMessage, string successTitle = "Успешно",
        string errorTitle = "Ошибка")
    {
        switch (resource.IsSuccess)
        {
            case true:
                await ShowMessage(successTitle, successMessage);
                break;
            case false:
                await ShowMessage(errorTitle, resource.ExceptionMessage ?? "Неизвестная ошибка");
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