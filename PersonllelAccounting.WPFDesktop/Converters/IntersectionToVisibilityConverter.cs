using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class IntersectionToVisibilityConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return Visibility.Visible;

        var target = values[0];
        var other = values.Skip(1).ToArray();

        return other.Any(x => Equals(x, target)) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}