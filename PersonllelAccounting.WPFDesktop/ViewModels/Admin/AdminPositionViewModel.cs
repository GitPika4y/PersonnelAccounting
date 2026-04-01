using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models.Main;
using WPF_Desktop.UseCases;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.Admin;

public partial class AdminPositionViewModel: ViewModelBase
{
    private readonly IPositionUseCase _useCase;

    public ObservableCollection<Position> Positions { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditPositionCommand))]
    private Position? _selectedPosition;

    public AdminPositionViewModel(IPositionUseCase useCase)
    {
        _useCase = useCase;
        _ = UpdatePositions();
    }

    private bool CanEdit() => SelectedPosition is not null;

    private async Task UpdatePositions()
    {
        var resource = await _useCase.GetAllAsync();
        HandleResource(
            resource,
            positions => UpdateObservableCollection(Positions, positions.ToList()));
    }

    [RelayCommand]
    private async Task AddPosition()
    {
        var dialogResult = await ShowDialog(new AdminPositionAddEditModalViewModel());
        switch (dialogResult)
        {
            case Resource<Position> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.AddAsync(successResource.Data);
                HandleResourceMessage(resource, "Должность успешно добавлена");
                await UpdatePositions();
                break;

            case Resource<Position> {IsSuccess: false, ExceptionMessage: not null} failedResource:
                HandleResourceMessage(failedResource, "");
                break;
        }
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private async Task EditPosition()
    {
        if (SelectedPosition is null)
        {
            ShowMessage("Редактирование невозможно", "Чтобы отредактировать, выберите запись");
            return;
        }

        var dialogResult = await ShowDialog(new AdminPositionAddEditModalViewModel(SelectedPosition));
        switch (dialogResult)
        {
            case Resource<Position> {IsSuccess: true, Data: not null} successResource:
                var resource = await _useCase.UpdateAsync(successResource.Data);
                HandleResourceMessage(resource, "Редактирование должности успешно");
                await UpdatePositions();
                break;

            case Resource<Position> {IsSuccess: false, ExceptionMessage: not null} failedResource:
                HandleResourceMessage(failedResource, "");
                break;
        }
    }
}