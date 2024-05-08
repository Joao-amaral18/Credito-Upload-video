using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CodenApp.Sdk.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .ToDictionary(
                    prop => prop?.GetCustomAttribute<XmlElementAttribute>()?.ElementName ?? prop.Name,
                    prop => (string)prop.GetValue(obj, null)
                );
        }
    }
}