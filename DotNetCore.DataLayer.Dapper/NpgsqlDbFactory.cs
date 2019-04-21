using Npgsql;
using System.Data;

namespace DotNetCore.DataLayer.Dapper
{
    public class NpgsqlDbFactory : DbFactory
    {

        public NpgsqlDbFactory() : base($"User ID=postgres;Password=root;Host=localhost;Port=5432;Database=my_db;Pooling=true;")
        {

        }

        public override IDbConnection Connection(string connectionString)
        {
            return new NpgsqlConnection($"User ID=postgres;Password=root;Host=localhost;Port=5432;Database=my_db;Pooling=true;");
        }
    }
}
