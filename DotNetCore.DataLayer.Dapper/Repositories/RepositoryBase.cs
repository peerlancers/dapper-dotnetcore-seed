using Dapper;
using DotNetCore.DataLayer.Dapper.Handlers;
using DotNetCore.DataLayer.Extensions;
using System;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, IDisposable, new()
    {
        protected readonly IDbFactory dbFactory;

        protected readonly IDbTransaction dbTransaction;

        public IDbFactory Db => dbFactory;

        public IDbTransaction DbTransaction => dbTransaction;

        public abstract string TableName { get; }

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

        public async virtual Task<TEntity> InsertAsync(TEntity record)
        {
            record.CreatedOn = DateTime.UtcNow;

            var (sql, param) = record.ToInsertData();

            await ExecuteAsync(sql, param);

            return record;
        }

        public async virtual Task<int> UpdateAsync(TEntity record)
        {
            record.LastUpdatedOn = DateTime.UtcNow;

            var (sql, param) = record.ToUpdateData();

            int affected = await ExecuteAsync(sql, param);

            return affected;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE id = @id";

            var record = await QueryFirstOrDefaultAsync(sql, new { id });

            return record;
        }

        public virtual async Task<int> DeleteAsync(TEntity record)
        {
            if (record == null) { return 0; }

            return await DeleteAsync(record.Id);
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM {TableName} WHERE id = @id;";
            var param = new { id };

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
