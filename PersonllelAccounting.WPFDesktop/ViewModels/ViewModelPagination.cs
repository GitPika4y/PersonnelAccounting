using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models;

namespace WPF_Desktop.ViewModels;

public abstract partial class ViewModelPagination<T>: ViewModelBase where T: EntityModel
{
    public static IEnumerable<int> PageSizes { get; } = [3, 10, 25];

    [ObservableProperty] int _selectedPage = 1;
    [ObservableProperty] private int _selectedPageSize = PageSizes.First();
    [ObservableProperty] private PaginationModel<T> _pagination;

    protected abstract Task UpdatePaginationCollection();

    async partial void OnSelectedPageSizeChanged(int value)
    {
        SelectedPage = 1;
        await UpdatePaginationCollection();
    }

    [RelayCommand]
    private async Task ChangePage(int page)
    {
        if (page < 1) page = 1;
        SelectedPage = page;
        await UpdatePaginationCollection();
    }
}