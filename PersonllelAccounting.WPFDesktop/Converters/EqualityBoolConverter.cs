using System.Globalization;
using System.Windows.Data;

namespace WPF_Desktop.Converters;

public class EqualityBoolConverter : IMultiValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

        if (values.Length < 2)
            return false;

        var obj1 = values[0];
        var obj2 = values[1];


        var areEqual = Equals(obj1, obj2);

        return Invert ? !areEqual : areEqual;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}