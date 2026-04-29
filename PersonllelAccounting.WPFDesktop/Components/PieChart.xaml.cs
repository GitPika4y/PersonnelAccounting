using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Desktop.Components;

public partial class PieChart : UserControl
{
    public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
        nameof(Series), typeof(IEnumerable), typeof(PieChart), new PropertyMetadata(default(IEnumerable)));

    public IEnumerable Series
    {
        get { return (IEnumerable)GetValue(SeriesProperty); }
        set { SetValue(SeriesProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(PieChart), new PropertyMetadata(default(string)));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register(
        nameof(TextSize), typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

    public double TextSize
    {
        get { return (double)GetValue(TextSizeProperty); }
        set { SetValue(TextSizeProperty, value); }
    }
    
    public PieChart()
    {
        InitializeComponent();
    }
}