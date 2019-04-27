using DotNetCore.DataLayer.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace DotNetCore.DataLayer.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsValidSqlField(this PropertyInfo prop)
        {
            return prop.CanRead && prop.CanWrite && !Attribute.IsDefined(prop, typeof(DbIgnoreAttribute));
        }

        public static string GetFieldName(this PropertyInfo propertyInfo, bool useSnakeCase = false)
        {
            bool hasSpecifiedFieldName = Attribute.IsDefined(propertyInfo, typeof(DbFieldNameAttribute));

            var fieldName = useSnakeCase ? propertyInfo.Name.ToSnakeCase() : propertyInfo.Name;
            if (hasSpecifiedFieldName)
            {
                if (propertyInfo.GetCustomAttributes(typeof(DbFieldNameAttribute), true).FirstOrDefault() is DbFieldNameAttribute attribute)
                {
                    fieldName = attribute.FieldName;
                }
            }

            return fieldName;
        }
    }
}
