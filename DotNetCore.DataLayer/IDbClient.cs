using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer
{
    public interface IDbClient : IDisposable
    {
        Task<List<TReturn>> QueryAsync<TReturn>(string sql, object param = null);

        Task<int> ExecuteAsync(string sql, object param = null);

        Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string sql, object param = null);

        Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null);
    }
}
