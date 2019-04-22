using DotNetCore.DataLayer.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using static DotNetCore.DataLayer.Extensions.StringExtensions;

namespace DotNetCore.DataLayer.Extensions
{
    public static class EntityExtensions
    {
        public static string GetTableName(this IEntity entity, CaseType caseType = CaseType.SnakeCase)
        {
            Type entityType = entity.GetType();
            bool hasSpecifiedTableName = Attribute.IsDefined(entityType, typeof(DbTableNameAttribute));
            var tableName = $"{entityType.Name.PascalCaseTo(caseType)}s";

            if (hasSpecifiedTableName)
            {
                DbTableNameAttribute dnAttribute = entityType.GetCustomAttributes(typeof(DbTableNameAttribute), true).FirstOrDefault() as DbTableNameAttribute;
                if (dnAttribute != null)
                {
                    tableName = dnAttribute.TableName;
                }
            }
            
            return tableName;
        }

        public static (string sql, object param) ToInsertData(this IEntity entity, CaseType caseType = CaseType.SnakeCase)
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
                    fields.Append($"{delimiter}{propertyInfo.Name.PascalCaseTo(caseType)}");
                    param.Append($"{delimiter}@{propertyInfo.Name.PascalCaseTo(caseType)}");
                    delimiter = ", ";

                    paramObject.Add(propertyInfo.Name.PascalCaseTo(caseType), propertyInfo.GetValue(entity));
                }
            }

            string sql = $"INSERT INTO {entity.GetTableName()} ({fields.ToString()}) VALUES({param.ToString()});";

            return (sql, paramObject);
        }

        public static (string sql, object param) ToUpdateData(this IEntity entity, CaseType caseType = CaseType.SnakeCase)
        {
            var fields = new StringBuilder();
            var paramObject = (IDictionary<string, object>)new ExpandoObject();

            Type entityType = entity.GetType();
            var delimiter = "";
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                if (propertyInfo.IsValidSqlField())
                {
                    fields.Append($"{delimiter}{propertyInfo.Name.PascalCaseTo(caseType)} = @{propertyInfo.Name.PascalCaseTo(caseType)}");
                    delimiter = ", ";
                    paramObject.Add(propertyInfo.Name.PascalCaseTo(caseType), propertyInfo.GetValue(entity));
                }
            }

            string sql = $"UPDATE {entity.GetTableName()} SET {fields.ToString()} WHERE id = @{"Id".PascalCaseTo(caseType)};";

            return (sql, paramObject);
        }
    }
}
