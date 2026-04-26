using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class EqualityToVisibilityMultiConverter: IMultiValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return Visibility.Collapsed;

        var areEqual = Equals(values[0], values[1]);

        if (Invert)
            areEqual = !areEqual;

        return areEqual
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}