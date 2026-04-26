using Data.Models.Main;

namespace WPF_Desktop.ViewModels.User;

public class OrderDetailModalViewModel(Order order): ViewModelBase
{
    public Order Order { get; } = order;

}