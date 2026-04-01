using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WPF_Desktop.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        if (member is null) return value.ToString();

        var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? value.ToString();
    }
}