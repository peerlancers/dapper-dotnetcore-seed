using DotNetCore.DataLayer.Attributes;
using System;
using System.Reflection;

namespace DotNetCore.DataLayer.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsValidSqlField(this PropertyInfo prop)
        {
            return prop.CanRead && prop.CanWrite && !Attribute.IsDefined(prop, typeof(DbIgnoreAttribute));
        }
    }
}
