using System.Globalization;
using System.Windows.Data;
using Data.Models.Auth;

namespace WPF_Desktop.Converters;

public class UsernameOrLoginConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not User user)
            return value?.ToString();

        return user.Username ?? user.Login;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}