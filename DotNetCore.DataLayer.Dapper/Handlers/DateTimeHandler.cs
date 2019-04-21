using Dapper;
using System;
using System.Data;

namespace DotNetCore.DataLayer.Dapper.Handlers
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }

        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
