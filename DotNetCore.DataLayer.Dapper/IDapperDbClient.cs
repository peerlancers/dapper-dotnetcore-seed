using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper
{
    public interface IDapperDbClient : IDbClient
    {
        Task<List<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "id");

        Task<GridReader> QueryMultipleAsync(string sql, object param = null);
    }
}
