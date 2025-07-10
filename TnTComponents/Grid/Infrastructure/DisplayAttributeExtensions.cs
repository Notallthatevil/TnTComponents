using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TnTComponents.Grid.Infrastructure;

internal static class DisplayAttributeExtensions {

    public static string? GetDisplayAttributeString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type itemType, string propertyName) {
        var propertyInfo = itemType.GetProperty(propertyName);
        //if (PropertyInfo == null && typeof(ICustomTypeProvider).IsAssignableFrom(itemType))
        //    PropertyInfo = ((ICustomTypeProvider)Item).GetCustomType().GetProperty(PropertyName);
        if (propertyInfo == null) {
            return null;
        }

        var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        if (displayAttribute is not null) {
            return displayAttribute.GetName();
        }
        else {
            var metadata = itemType.GetCustomAttribute<MetadataTypeAttribute>();
            if (metadata is not null) {
                return metadata.MetadataClassType.GetDisplayAttributeString(propertyName);
            }
        }
        return null;
    }
}