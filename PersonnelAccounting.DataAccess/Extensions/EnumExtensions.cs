using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Data.Extensions;

public static class EnumExtensions
{
    public static string EnumToSqlQuery<T>(Type enumType, Expression<Func<T, object>> enumPropertyExpression)
    {
        var propName = enumPropertyExpression.GetPropertyName();
        var values = Enum.GetNames(enumType);

        return $"[{propName}] IN ({string.Join(',', values.Select(v => $"'{v}'"))})";
    }

    private static string GetDisplayName(this Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (member is null) return value.ToString();

        var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? value.ToString();
    }
}