using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace WPF_Desktop.ViewModels.Admin;

public class SideBarItem
{
    public SideBarItem(PackIconKind iconKind, string title, Type destination, ICommand navigateCommand)
    {
        IconKind = iconKind;
        Title = title;
        Destination = destination;
        NavigateCommand = navigateCommand;
    }

    public PackIconKind IconKind { get; set; }
    public string Title { get; set; }
    public Type Destination { get; set; }
    public ICommand NavigateCommand { get; set; }
}