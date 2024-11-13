using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarriageAgency.Infrastructure
{
    // Transformation of a dictionary to an object
    public static class Transformations
    {
        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                Type tPropertyType = property.PropertyType;

                // Skip empty strings for non-string properties
                if (string.IsNullOrEmpty(item.Value) && tPropertyType != typeof(string))
                {
                    if (Nullable.GetUnderlyingType(tPropertyType) != null || !tPropertyType.IsValueType)
                    {
                        property.SetValue(t, null);
                    }
                    else
                    {
                        property.SetValue(t, Activator.CreateInstance(tPropertyType));
                    }
                    continue;
                }

                // Handle DateOnly explicitly
                Type targetType = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                object newValue = null;

                try
                {
                    if (targetType == typeof(DateOnly))
                    {
                        newValue = DateOnly.Parse(item.Value); // Parse the string to DateOnly
                    }
                    else
                    {
                        newValue = Convert.ChangeType(item.Value, targetType);
                    }
                }
                catch (FormatException)
                {
                    // Handle invalid date format or conversion error
                    newValue = null;
                }

                property.SetValue(t, newValue);
            }
            return t;
        }
    }
}
