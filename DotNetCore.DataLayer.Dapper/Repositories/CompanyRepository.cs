using Dapper;
using DotNetCore.DataLayer.Entities;
using DotNetCore.DataLayer.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbFactory dbFactory, IDbTransaction dbTransaction) : base(dbFactory, dbTransaction)
        {
        }

        public override string TableName => "companies";

        public async override Task<Company> InsertAsync(Company record)
        {
            record.CreatedOn = DateTime.UtcNow;

            #region SQL

            const string sql = @"
            INSERT INTO 
                companies
			(
				id,
                name,
                description,
                created_on,
                last_updated_on
			)
		    VALUES 
			(
				@p_id,
                @p_name,
                @p_description,
                @p_created_on,
                @p_last_updated_on
			);";

            #endregion

            #region Params

            var param = new DynamicParameters();
            param.Add("p_id", record.Id);
            param.Add("p_name", record.Name);
            param.Add("p_description", record.Description);
            param.Add("p_created_on", record.CreatedOn);
            param.Add("p_last_updated_on", record.LastUpdatedOn);

            #endregion

            await ExecuteAsync(sql, param);

            return record;
        }

        public async override Task<int> UpdateAsync(Company record)
        {
            record.LastUpdatedOn = DateTime.UtcNow;

            #region SQL

            const string sql = @"
            UPDATE
                companies
			SET
                name = @p_name,
                description = @p_description,
                created_on = @p_created_on,
                last_updated_on = @p_last_updated_on
            WHERE
                id = @p_id;";

            #endregion

            #region Params

            var param = new DynamicParameters();
            param.Add("p_id", record.Id);
            param.Add("p_name", record.Name);
            param.Add("p_description", record.Description);
            param.Add("p_created_on", record.CreatedOn);
            param.Add("p_last_updated_on", record.LastUpdatedOn);

            #endregion

            return await ExecuteAsync(sql, param);
        }
    }
}
