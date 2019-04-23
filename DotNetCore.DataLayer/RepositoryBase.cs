using DotNetCore.DataLayer.Extensions;
using System;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, IDisposable, new()
    {
        private readonly IDbClient dbClient;

        public RepositoryBase(IDbClient dbClient)
        {
            this.dbClient = dbClient;
        }

        public void Dispose()
        {
            dbClient.Dispose();
        }

        public async virtual Task<int> InsertAsync(TEntity record)
        {
            record.Id = record.Id == Guid.Empty ? Guid.NewGuid() : record.Id;
            record.CreatedOn = DateTime.UtcNow;

            var (sql, param) = record.ToInsertData(useSnakeCase: true);

            var affected = await dbClient.ExecuteAsync(sql, param);

            return affected;
        }

        public async virtual Task<int> UpdateAsync(TEntity record)
        {
            record.LastUpdatedOn = DateTime.UtcNow;

            var (sql, param) = record.ToUpdateData(useSnakeCase: true);

            int affected = await dbClient.ExecuteAsync(sql, param);

            return affected;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE id = @id";

            var record = await dbClient.QueryFirstOrDefaultAsync<TEntity>(sql, new { id });

            return record;
        }

        public async Task<int> DeleteAsync(TEntity record)
        {
            if (record == null) { return 0; }

            return await DeleteAsync(record.Id);
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM {TableName} WHERE id = @id;";
            var param = new { id };

            return await dbClient.ExecuteAsync(sql, param);
        }

        public abstract string TableName { get; }
    }
}
