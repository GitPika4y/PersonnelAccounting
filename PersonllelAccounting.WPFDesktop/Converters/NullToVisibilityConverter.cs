using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class NullToVisibilityConverter: IValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var notNull = value is not null;

        if (Invert)
            notNull = !notNull;

        return notNull ? Visibility.Visible : Visibility.Collapsed;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}