using System;

namespace DotNetCore.DataLayer.Attributes
{
    public class DbTableNameAttribute : Attribute
    {
        public string TableName { get; private set; }

        public DbTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
