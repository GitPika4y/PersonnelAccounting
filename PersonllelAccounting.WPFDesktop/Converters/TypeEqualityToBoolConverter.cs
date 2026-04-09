using System.Globalization;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class TypeEqualityToBoolConverter : IMultiValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2) return true;

        if (values[0] is null || values[1] is null) return true;

        var type1 = values[0].GetType();
        var type2 = values[1].GetType();

        var areEqual = type2 == type1;
        return Invert ? !areEqual : areEqual;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}