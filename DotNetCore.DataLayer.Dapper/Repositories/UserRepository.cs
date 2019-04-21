using Dapper;
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
        public UserRepository(IDbFactory dbFactory, IDbTransaction dbTransaction) : base(dbFactory, dbTransaction)
        {
        }

        public override string TableName => "users";

        public async override Task<User> InsertAsync(User record)
        {
            record.CreatedOn = DateTime.UtcNow;

            #region SQL

            const string sql = @"
            INSERT INTO 
                users
			(
				id,
                username,
                first_name,
                last_name,
                email,
                status,
                company_id,
                created_on,
                last_updated_on
			)
		    VALUES 
			(
				@p_id,
                @p_username,
                @p_first_name,
                @p_last_name,
                @p_email,
                @p_status,
                @p_company_id,
                @p_created_on,
                @p_last_updated_on
			);";

            #endregion

            #region Params

            var param = new DynamicParameters();
            param.Add("p_id", record.Id);
            param.Add("p_username", record.Username);
            param.Add("p_first_name", record.FirstName);
            param.Add("p_last_name", record.LastName);
            param.Add("p_email", record.Email);
            param.Add("p_status", record.Status);
            param.Add("p_company_id", record.CompanyId);
            param.Add("p_created_on", record.CreatedOn);
            param.Add("p_last_updated_on", record.LastUpdatedOn);

            #endregion

            await ExecuteAsync(sql, param);

            return record;
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

        public async override Task<int> UpdateAsync(User record)
        {
            record.LastUpdatedOn = DateTime.UtcNow;

            #region SQL

            const string sql = @"
            UPDATE
                users
			SET
                username = @p_username,
                first_name = @p_first_name,
                last_name = @p_last_name,
                email = @p_email,
                status = @p_status,
                company_id = @p_company_id,
                created_on = @p_created_on,
                last_updated_on = @p_last_updated_on
            WHERE
                id = @p_id;";

            #endregion

            #region Params

            var param = new DynamicParameters();
            param.Add("p_id", record.Id);
            param.Add("p_username", record.Username);
            param.Add("p_first_name", record.FirstName);
            param.Add("p_last_name", record.LastName);
            param.Add("p_email", record.Email);
            param.Add("p_status", record.Status);
            param.Add("p_company_id", record.CompanyId);
            param.Add("p_created_on", record.CreatedOn);
            param.Add("p_last_updated_on", record.LastUpdatedOn);
            
            #endregion

            return await ExecuteAsync(sql, param);
        }
    }
}
