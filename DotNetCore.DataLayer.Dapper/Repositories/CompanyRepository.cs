using DotNetCore.DataLayer.Entities;
using DotNetCore.DataLayer.Interfaces;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public class CompanyRepository : DapperRepositoryBase<Company>, ICompanyRepository
    {
        public override string TableName => TableNames.Companies;

        public CompanyRepository(IDapperDbClient dbClient) : base(dbClient)
        {
        }
    }
}
