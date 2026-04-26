using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Desktop.Views.User.Includes;

public partial class OrderCard : UserControl
{
    public static readonly DependencyProperty OnDetailClickCommandProperty = DependencyProperty.Register(
        nameof(OnDetailClickCommand), typeof(ICommand), typeof(OrderCard), new PropertyMetadata(default(ICommand)));

    public ICommand OnDetailClickCommand
    {
        get { return (ICommand)GetValue(OnDetailClickCommandProperty); }
        set { SetValue(OnDetailClickCommandProperty, value); }
    }

    public OrderCard()
    {
        InitializeComponent();
    }
}