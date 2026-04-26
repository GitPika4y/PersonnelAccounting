using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Desktop.Views.User.Includes;

public partial class EmployeeCard : UserControl
{
    public static readonly DependencyProperty OnDetailClickCommandProperty = DependencyProperty.Register(
        nameof(OnDetailClickCommand), typeof(ICommand), typeof(EmployeeCard), new PropertyMetadata(default(ICommand)));

    public ICommand OnDetailClickCommand
    {
        get { return (ICommand)GetValue(OnDetailClickCommandProperty); }
        set { SetValue(OnDetailClickCommandProperty, value); }
    }

    public EmployeeCard()
    {
        InitializeComponent();
    }
}