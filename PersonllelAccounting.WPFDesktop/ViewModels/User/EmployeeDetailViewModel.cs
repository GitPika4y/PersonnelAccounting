using CommunityToolkit.Mvvm.ComponentModel;
using Data.Models.Main;
using WPF_Desktop.Utils;

namespace WPF_Desktop.ViewModels.User;

public partial class EmployeeDetailViewModel: ViewModelBase, INavigationAware<Employee>
{
    [ObservableProperty] private Employee _employee = null!;

    public void OnNavigatedTo(Employee parameter)
    {
        Employee = parameter;
    }
}