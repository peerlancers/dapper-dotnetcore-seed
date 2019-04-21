using Dapper;
using DotNetCore.DataLayer.Dapper.Handlers;
using System;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, IDisposable, new()
    {
        public abstract string TableName { get; }

        protected readonly IDbFactory dbFactory;

        protected readonly IDbTransaction dbTransaction;

        public IDbFactory Db => dbFactory;

        public IDbTransaction DbTransaction => dbTransaction;

        public RepositoryBase(IDbFactory dbFactory, IDbTransaction dbTransaction)
        {
            AddTypeHandler(new DateTimeHandler());
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.dbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
        }

        public void Dispose()
        {
            dbTransaction.Dispose();
            dbFactory.Dispose();
        }

        public abstract Task<TEntity> InsertAsync(TEntity record);

        public abstract Task<int> UpdateAsync(TEntity record);

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE id = @p_id";

            var record = await QueryFirstOrDefaultAsync(sql, new { p_id = id });

            return record;
        }

        public async Task<int> DeleteAsync(TEntity record)
        {
            if (record == null) { return 0; }

            return await DeleteAsync(record.Id);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM {TableName} WHERE id = @p_id;";
            var param = new { p_id = id };

            return await Db.Context().ExecuteAsync(sql, param, DbTransaction);
        }

        protected async Task<GridReader> QueryMultipleAsync(string sql, object param = null)
        {
            return await Db.Context().QueryMultipleAsync(sql, param, DbTransaction);
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            return await Db.Context().ExecuteAsync(sql, param, DbTransaction);
        }

        protected async Task<TEntity> QueryFirstOrDefaultAsync(string sql, object param = null)
        {
            return await Db.Context().QueryFirstOrDefaultAsync<TEntity>(sql, param, DbTransaction);
        }

        protected async Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string sql, object param = null)
        {
            return await Db.Context().QueryFirstOrDefaultAsync<TReturn>(sql, param, DbTransaction);
        }

        protected async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null)
        {
            return await Db.Context().ExecuteScalarAsync<TReturn>(sql, param, DbTransaction);
        }
    }
}
