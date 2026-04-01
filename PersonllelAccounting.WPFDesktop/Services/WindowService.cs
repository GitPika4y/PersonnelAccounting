namespace WPF_Desktop.Services;

public class WindowService(MainWindow window): IWindowService
{
    public void SetSize(int width, int height)
    {
        window.Width = width;
        window.Height = height;
    }

    public void SetTitle(string title) => window.Title = title;

    public void SetWidth(int width) => window.Width = width;

    public void SetHeight(int height) => window.Height = height;
    public void SetPosition(int x, int y)
    {
        window.Left = x;
        window.Top = y;
    }

    public void SetMinSize(int width, int height)
    {
        window.MinWidth = width;
        window.MinHeight = height;
    }

    public void SetMinWidth(int width) => window.MinWidth = width;

    public void SetMinHeight(int height) => window.MinHeight = height;

    public void SetMaxSize(int width, int height)
    {
        window.MaxWidth = width;
        window.MaxHeight = height;
    }

    public void SetMaxWidth(int width) => window.MaxWidth = width;

    public void SetMaxHeight(int height) => window.MaxHeight = height;
}