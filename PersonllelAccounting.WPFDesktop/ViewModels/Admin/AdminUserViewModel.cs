using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_Desktop.Services;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminUserViewModel: ViewModelBase
{
    private readonly IUserUseCase _useCase;
    private readonly ISessionService _sessionService;

    public ObservableCollection<Data.Models.Auth.User> Users { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditUserCommand))]
    private Data.Models.Auth.User? _selectedUser;

    public AdminUserViewModel(IUserUseCase useCase, ISessionService sessionService)
    {
        _useCase = useCase;
        _sessionService = sessionService;
         _ = UpdateUserCollection();
    }

    private async Task UpdateUserCollection()
    {
        var resource = await _useCase.GetAll();
        await HandleResource(
            resource,
            users => UpdateObservableCollection(Users, users));
    }

    private bool CanEdit() => SelectedUser is not null && _sessionService.GetCurrentUser().Id != SelectedUser.Id;


    [RelayCommand]
    private async Task AddUser()
    {
        var dialogResult = await ShowDialog(new AdminUserAddEditModalViewModel());

        switch (dialogResult)
        {
            case Resource<Data.Models.Auth.User> { IsSuccess: true, Data: not null } successResource:
            {
                var resource = await _useCase.Add(successResource.Data);
                await HandleResourceMessage(resource, "Пользователь успешно добавлен");
                await UpdateUserCollection();
                break;
            }
            case Resource<Data.Models.Auth.User> { IsSuccess: false } failureResource:
                await HandleResourceMessage(failureResource, "");
                break;
        }
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private async Task EditUser()
    {
        if (SelectedUser is null)
        {
            await ShowMessage("Редактирование невозможно", "Выберите пользователя чтобы редактировать");
            return;
        }

        var dialogResult = await ShowDialog(new AdminUserAddEditModalViewModel(SelectedUser));

        switch (dialogResult)
        {
            case Resource<Data.Models.Auth.User> { IsSuccess: true, Data: not null } successResource:
                var resource = await _useCase.Update(successResource.Data);
                await HandleResourceMessage(resource, "Пользователь успешно добавлен");
                await UpdateUserCollection();
                break;

            case Resource<Data.Models.Auth.User> { IsSuccess: false } failureResource:
                await HandleResourceMessage(failureResource, "");
                break;
        }
    }
}