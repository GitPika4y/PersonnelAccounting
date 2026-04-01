namespace WPF_Desktop.Services;

public interface IWindowService
{
    void SetSize(int width, int height);
    void SetTitle(string title);
    void SetWidth(int width);
    void SetHeight(int height);
    void SetPosition(int x, int y);
    void SetMinSize(int  width, int height);
    void SetMinWidth(int width);
    void SetMinHeight(int height);
    void SetMaxSize(int  width, int height);
    void SetMaxWidth(int width);
    void SetMaxHeight(int height);
}