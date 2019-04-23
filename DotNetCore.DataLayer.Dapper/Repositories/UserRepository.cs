using DotNetCore.DataLayer.Entities;
using DotNetCore.DataLayer.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public class UserRepository : DapperRepositoryBase<User>, IUserRepository
    {
        public override string TableName => TableNames.Users;

        public UserRepository(IDapperDbClient dbClient) : base(dbClient)
        {
        }

        public async override Task<User> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE id = @id";
            var record = await DbClient.QueryFirstOrDefaultAsync<User>(sql, new { id });
            if (record == null) { return record; }

            var multipleQuery = SharedQueries.Companies.GetCompanyById("company_id");
            var multipleResults = await DbClient.QueryMultipleAsync(
                multipleQuery,
                new
                {
                    company_id = record.CompanyId
                });

            record.CompanyInfo = (await multipleResults.ReadAsync<Company>()).SingleOrDefault();

            return record;
        }
    }
}
