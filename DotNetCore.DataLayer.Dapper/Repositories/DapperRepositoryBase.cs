using Dapper;
using DotNetCore.DataLayer.Dapper.Handlers;
using System;
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
    }
}
