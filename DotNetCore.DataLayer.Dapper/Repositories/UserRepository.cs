using DotNetCore.DataLayer.Entities;
using DotNetCore.DataLayer.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public override string TableName => "users";

        public UserRepository(IDbFactory dbFactory, IDbTransaction dbTransaction) : base(dbFactory, dbTransaction)
        {
        }

        public async override Task<User> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE id = @p_id";
            var record = await QueryFirstOrDefaultAsync(sql, new { p_id = id });
            if (record == null) { return record; }

            var multipleSql = $@"SELECT * FROM companies WHERE id = @p_company_id;";
            var multipleResults = await QueryMultipleAsync(
                multipleSql,
                new
                {
                    p_company_id = record.CompanyId
                });

            record.CompanyInfo = (await multipleResults.ReadAsync<Company>()).SingleOrDefault();

            return record;
        }
    }
}
