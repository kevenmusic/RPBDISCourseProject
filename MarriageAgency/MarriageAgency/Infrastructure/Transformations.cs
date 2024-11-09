using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarriageAgency.Infrastructure
{
    //Преобразование словаря в объект
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

                // Fix nullables and convert the value
                Type targetType = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                object newValue = Convert.ChangeType(item.Value, targetType);
                property.SetValue(t, newValue);
            }
            return t;
        }
    }
}
