using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Models;

namespace WPF_Desktop.ViewModels;

public abstract partial class ViewModelPagination<T>: ViewModelBase where T: EntityModel
{
    public static IEnumerable<int> PageSizes { get; } = [1, 2, 5];

    protected int SelectedPage = 1;
    [ObservableProperty] private int _selectedPageSize = PageSizes.First();
    [ObservableProperty] private PaginationModel<T> _pagination;


    protected abstract Task UpdateCollection();

    [RelayCommand]
    private async Task ChangePage(int page)
    {
        if (page < 1) page = 1;
        SelectedPage = page;
        await UpdateCollection();
    }

    [RelayCommand]
    private async Task ChangePageSize(int pageSize)
    {
        SelectedPageSize = pageSize;
        SelectedPage = 1;
        await UpdateCollection();
    }
}