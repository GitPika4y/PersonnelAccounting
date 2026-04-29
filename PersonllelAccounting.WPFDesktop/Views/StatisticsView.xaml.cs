using System.Windows;
using System.Windows.Controls;
using WPF_Desktop.ViewModels;

namespace WPF_Desktop.Views;

public partial class StatisticsView : UserControl
{
    public StatisticsView()
    {
        InitializeComponent();
    }

    private async void StatisticsView_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is StatisticsViewModel vm)
            await vm.InitializeAsync();
    }
}