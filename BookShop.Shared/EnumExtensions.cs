using System.ComponentModel;

namespace BookShop.Shared;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attr?.Description ?? value.ToString();
    }

    public static string GetName(this Enum value) => value.ToString();
}