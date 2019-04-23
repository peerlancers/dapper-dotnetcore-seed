using System;

namespace DotNetCore.DataLayer.Attributes
{
    public class DbFieldNameAttribute : Attribute
    {
        public string FieldName { get; private set; }

        public DbFieldNameAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
