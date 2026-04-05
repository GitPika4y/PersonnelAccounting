using System.Windows;
using System.Windows.Controls;

namespace WPF_Desktop.Components;

public partial class KeyValueVerticalGrid : UserControl
{
    public static readonly DependencyProperty UseTextBoxProperty = DependencyProperty.Register(nameof(UseTextBox), typeof(bool), typeof(KeyValueVerticalGrid), new PropertyMetadata(true));

    public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
        nameof(Key), typeof(string), typeof(KeyValueVerticalGrid), new PropertyMetadata(default(string)));

    public string Key
    {
        get { return (string)GetValue(KeyProperty); }
        set { SetValue(KeyProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(string), typeof(KeyValueVerticalGrid), new PropertyMetadata(default(string)));

    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public bool UseTextBox
    {
        get => (bool)GetValue(UseTextBoxProperty);
        set => SetValue(UseTextBoxProperty, value);
    }

    public KeyValueVerticalGrid()
    {
        InitializeComponent();
    }
}