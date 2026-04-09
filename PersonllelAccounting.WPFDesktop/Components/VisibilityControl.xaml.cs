using System.Windows;
using System.Windows.Controls;

namespace WPF_Desktop.Components;

public partial class VisibilityControl : UserControl
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(VisibilityControl), new PropertyMetadata(default(object)));

    public object Value
    {
        get { return (object)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty TargetValueProperty = DependencyProperty.Register(
        nameof(TargetValue), typeof(object), typeof(VisibilityControl), new PropertyMetadata(default(object)));

    public object TargetValue
    {
        get { return (object)GetValue(TargetValueProperty); }
        set { SetValue(TargetValueProperty, value); }
    }

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content), typeof(object), typeof(VisibilityControl), new PropertyMetadata(default(object)));

    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public VisibilityControl()
    {
        InitializeComponent();
    }
}