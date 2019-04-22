using DotNetCore.DataLayer.Entities;
using DotNetCore.DataLayer.Interfaces;
using System.Data;

namespace DotNetCore.DataLayer.Dapper.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public override string TableName => "companies";

        public CompanyRepository(IDbFactory dbFactory, IDbTransaction dbTransaction) : base(dbFactory, dbTransaction)
        {
        }
    }
}
