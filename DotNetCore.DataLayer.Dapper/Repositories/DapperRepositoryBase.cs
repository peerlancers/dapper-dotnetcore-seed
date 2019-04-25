using Dapper;
using DotNetCore.DataLayer.Dapper.Handlers;
using System;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public abstract class DapperRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class, IEntity, IDisposable, new()
    {
        public IDapperDbClient DbClient { get; private set; }

        public DapperRepositoryBase(IDapperDbClient dbClient) : base(dbClient)
        {
            AddTypeHandler(new DateTimeHandler());
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            DbClient = dbClient ?? throw new ArgumentNullException(nameof(dbClient));
        }

        public async Task<long> GetTotalCountAsync(string filter, object param, string alias = "")
        {
            var sqlCount = $"SELECT COUNT(1) FROM {TableName} {alias} {filter};";
            return await DbClient.ExecuteScalarAsync<long>(sqlCount, param);
        }

        protected abstract string GetBaseQuery();
    }
}
