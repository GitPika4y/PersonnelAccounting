using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace WPF_Desktop.ViewModels;

public class SideBarItem
{
    public SideBarItem(PackIconKind iconKind, string title, Type destination)
    {
        IconKind = iconKind;
        Title = title;
        Destination = destination;
    }

    public PackIconKind IconKind { get; set; }
    public string Title { get; set; }
    public Type Destination { get; set; }
}