using DotNetCore.DataLayer.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetCore.DataLayer.Extensions
{
    public static class EntityExtensions
    {
        public static string GetTableName(this IEntity entity, bool useSnakeCase = false)
        {
            Type entityType = entity.GetType();
            bool hasSpecifiedTableName = Attribute.IsDefined(entityType, typeof(DbTableNameAttribute));
            var tableName = useSnakeCase ? $"{entityType.Name}s".ToSnakeCase() : $"{entityType.Name}s";

            if (hasSpecifiedTableName)
            {
                if (entityType.GetCustomAttributes(typeof(DbTableNameAttribute), true).FirstOrDefault() is DbTableNameAttribute attribute)
                {
                    tableName = attribute.TableName;
                }
            }
            
            return tableName;
        }

        public static (string sql, object param) ToInsertData(this IEntity entity, bool useSnakeCase = false)
        {
            var fields = new StringBuilder();
            var param = new StringBuilder();
            var paramObject = (IDictionary<string, object>)new ExpandoObject();

            Type entityType = entity.GetType();
            var delimiter = "";

            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                if (propertyInfo.IsValidSqlField())
                {
                    string fieldName = propertyInfo.GetFieldName(useSnakeCase);
                    fields.Append($"{delimiter}{fieldName}");
                    param.Append($"{delimiter}@{fieldName}");
                    delimiter = ", ";

                    paramObject.Add(fieldName, propertyInfo.GetValue(entity));
                }
            }

            string sql = $"INSERT INTO {entity.GetTableName(useSnakeCase)} ({fields.ToString()}) VALUES({param.ToString()});";

            return (sql, paramObject);
        }

        public static (string sql, object param) ToUpdateData(this IEntity entity, bool useSnakeCase = false)
        {
            var fields = new StringBuilder();
            var paramObject = (IDictionary<string, object>)new ExpandoObject();

            Type entityType = entity.GetType();
            var delimiter = "";
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                if (propertyInfo.IsValidSqlField())
                {
                    string fieldName = propertyInfo.GetFieldName(useSnakeCase);
                    fields.Append($"{delimiter}{fieldName} = @{fieldName}");
                    delimiter = ", ";
                    paramObject.Add(fieldName, propertyInfo.GetValue(entity));
                }
            }

            string idParam = useSnakeCase ? nameof(entity.Id).ToSnakeCase() : nameof(entity.Id);
            string sql = $"UPDATE {entity.GetTableName(useSnakeCase)} SET {fields.ToString()} WHERE id = @{idParam};";

            return (sql, paramObject);
        }
    }
}
