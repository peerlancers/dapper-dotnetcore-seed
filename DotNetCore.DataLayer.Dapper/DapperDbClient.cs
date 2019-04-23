using System;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper
{
    public class DapperDbClient : IDapperDbClient
    {
        protected readonly IDbFactory dbFactory;

        protected readonly IDbTransaction dbTransaction;

        public DapperDbClient(IDbFactory dbFactory, IDbTransaction dbTransaction)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.dbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
        }

        public void Dispose()
        {
            dbFactory.Dispose();
            dbTransaction.Dispose();
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null)
        {
            return await DbFactory.Context().QueryMultipleAsync(sql, param, DbTransaction);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            return await DbFactory.Context().ExecuteAsync(sql, param, DbTransaction);
        }

        public async Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string sql, object param = null)
        {
            return await DbFactory.Context().QueryFirstOrDefaultAsync<TReturn>(sql, param, DbTransaction);
        }

        public async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null)
        {
            return await DbFactory.Context().ExecuteScalarAsync<TReturn>(sql, param, DbTransaction);
        }

        public IDbFactory DbFactory => dbFactory;

        public IDbTransaction DbTransaction => dbTransaction;
    }
}
