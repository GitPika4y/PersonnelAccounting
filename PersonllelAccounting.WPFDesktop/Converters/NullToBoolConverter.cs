using System.Globalization;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class NullToBoolConverter: IValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isNull = value is null;

        if (Invert)
            isNull = !isNull;

        return isNull;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}