using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Desktop.Components;

public partial class PaginationControl : UserControl
{
    public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
        nameof(Count), typeof(int), typeof(PaginationControl), new PropertyMetadata(default(int)));

    public int Count
    {
        get { return (int)GetValue(CountProperty); }
        set { SetValue(CountProperty, value); }
    }

    public static readonly DependencyProperty PageSizesProperty = DependencyProperty.Register(
        nameof(PageSizes), typeof(IEnumerable<int>), typeof(PaginationControl), new PropertyMetadata(default(IEnumerable<int>)));

    public IEnumerable<int> PageSizes
    {
        get { return (IEnumerable<int>)GetValue(PageSizesProperty); }
        set { SetValue(PageSizesProperty, value); }
    }

    public static readonly DependencyProperty SelectedPageSizeProperty = DependencyProperty.Register(
        nameof(SelectedPageSize), typeof(int), typeof(PaginationControl), new PropertyMetadata(default(int)));

    public int SelectedPageSize
    {
        get { return (int)GetValue(SelectedPageSizeProperty); }
        set { SetValue(SelectedPageSizeProperty, value); }
    }

    public static readonly DependencyProperty ChangePageCommandProperty = DependencyProperty.Register(
        nameof(ChangePageCommand), typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand)));

    public ICommand ChangePageCommand
    {
        get { return (ICommand)GetValue(ChangePageCommandProperty); }
        set { SetValue(ChangePageCommandProperty, value); }
    }

    public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(
        nameof(Pages), typeof(IEnumerable<int>), typeof(PaginationControl), new PropertyMetadata(default(IEnumerable<int>)));

    public IEnumerable<int> Pages
    {
        get { return (IEnumerable<int>)GetValue(PagesProperty); }
        set { SetValue(PagesProperty, value); }
    }

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content), typeof(object), typeof(PaginationControl), new PropertyMetadata(default(object)));

    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register(
        nameof(SelectedPage), typeof(int), typeof(PaginationControl), new PropertyMetadata(default(int)));

    public int SelectedPage
    {
        get { return (int)GetValue(SelectedPageProperty); }
        set { SetValue(SelectedPageProperty, value); }
    }

    public static readonly DependencyProperty PageSizeChangedProperty = DependencyProperty.Register(
        nameof(PageSizeChanged), typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand)));

    public ICommand PageSizeChanged
    {
        get { return (ICommand)GetValue(PageSizeChangedProperty); }
        set { SetValue(PageSizeChangedProperty, value); }
    }

    public PaginationControl()
    {
        InitializeComponent();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        PageSizeChanged?.Execute(SelectedPageSize);
    }
}