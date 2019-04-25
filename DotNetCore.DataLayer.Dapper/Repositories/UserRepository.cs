using Dapper;
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
            var sql = SharedQueries.Users.GetUserById("id");
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

        public async Task<DbRecordList<User>> SearchAsync(
            string keyword, 
            Sorting<UserSortableFields> sorting = null,
            IPagingInfo paging = null)
        {
            var records = new DbRecordList<User>(paging);

            // Default sorting
            if (sorting == null)
            {
                sorting = new Sorting<UserSortableFields>(UserSortableFields.FirstName, SortDirection.Asc);
            }

            var param = new DynamicParameters();

            string filter = GetSqlFilter(keyword, param);

            var query = new SimpleQuery(GetBaseQuery())
                .SetFilter(filter)
                .SortBy(sorting)
                .Page(paging);

            var record = await DbClient.QueryAsync<User, Company, User>(
                query.ToString(), 
                (user, company) =>
                {
                    user.CompanyInfo = company;
                    return user;
                }, 
                param);

            if (record == null) { return records; }

            records.Records = record;
            records.TotalCount = await GetTotalCountAsync(filter, param, "u");

            return records;
        }

        protected override string GetBaseQuery()
        {
            return $"SELECT u.*, c.* FROM {TableName} u INNER JOIN {TableNames.Companies} c ON u.company_id = c.id";
        }

        private string GetSqlFilter(string keyword, DynamicParameters param)
        {
            param.Add("keyword", keyword ?? string.Empty);
            return string.IsNullOrEmpty(keyword)
                ? string.Empty
                : @"WHERE LOWER(u.username) LIKE '' || LOWER(@keyword) || '%'
                    OR LOWER(u.first_name) LIKE '' || LOWER(@keyword) || '%'
                    OR LOWER(u.last_name) LIKE '' || LOWER(@keyword) || '%'";
        }
    }
}
