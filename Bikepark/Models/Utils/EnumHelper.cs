
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bikepark.Models
{

    public static class EnumHelper<T>
        where T : struct, Enum // This constraint requires C# 7.3 or later.
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string LookupResource(Type? resourceManagerProvider, string? resourceKey)
        {
            if (resourceManagerProvider == null || resourceKey == null)
                return string.Empty;
                
            if (resourceManagerProvider == null)
                return resourceKey;

            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string), Array.Empty<Type>(), null);

            if (resourceKeyProperty != null)
            {
                var result = resourceKeyProperty.GetMethod?.Invoke(null, null) as string;
                return result ?? string.Empty;
            }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null)
                return value.ToString();

            if (fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) is not DisplayAttribute[] descriptionAttributes) return string.Empty;

            if (descriptionAttributes[0].ResourceType != null && descriptionAttributes.Length > 0)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            return value.ToString();
        }
    }
}
