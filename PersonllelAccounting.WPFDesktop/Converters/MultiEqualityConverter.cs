using System.Globalization;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class MultiEqualityConverter : IMultiValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.Length == 2 && Equals(values[0], values[1]) | Invert;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}