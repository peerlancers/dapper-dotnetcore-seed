using DotNetCore.DataLayer.Dapper;

namespace DotNetCore.DataLayer.DapperTests.Repositories
{
    public abstract class RepositoryTestBase
    {
        protected readonly IDbFactory DbFactory;
        protected readonly IUnitOfWork Work;

        public RepositoryTestBase()
        {
            var settings = new EnvironmentSettings();
            DbFactory = new NpgsqlDbFactory(settings);
            Work = new UnitOfWork(DbFactory);
        }
    }
}
