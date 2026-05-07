using System.Globalization;
using System.Windows.Data;
using WPF_Desktop.Extensions;

namespace WPF_Desktop.Converters;

public class EnumToDisplayNameConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
            return enumValue.GetDisplayName();

        return value?.ToString() ?? "---";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}