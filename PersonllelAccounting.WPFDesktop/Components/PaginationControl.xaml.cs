using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Desktop.Components;

public partial class PaginationControl : UserControl
{
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content), typeof(object), typeof(PaginationControl), new PropertyMetadata(default(object)));

    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public PaginationControl()
    {
        InitializeComponent();
    }
}