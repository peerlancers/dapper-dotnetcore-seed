using DotNetCore.DataLayer.Dapper;
using DotNetCore.DataLayer.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCore.DataLayer.DapperTests.Repositories
{
    public class CompanyRepositoryTest
    {
        private readonly IDbFactory dbFactory = new NpgsqlDbFactory();
        private readonly IUnitOfWork work;

        public CompanyRepositoryTest()
        {
            work = new UnitOfWork(dbFactory);
        }

        [Fact]
        public async Task CompanyRepository_ShouldWork()
        {
            Company companyEntity = new Company
            {
                Name = "The Company",
                Description = "The Company Description",
            };
            var company = await InsertCompany(companyEntity);
            company = await UpdateCompany(company);
            company = await RetrieveCompany(company.Id);
            await DeleteCompany(company);
        }

        private async Task<Company> InsertCompany(Company entity)
        {
            var result = await work.Companies.InsertAsync(entity);
            work.Save();

            return entity;
        }

        private async Task<Company> UpdateCompany(Company entity)
        {
            entity.Name = "XXX";
            entity.Description = "Description XXX";
            var affected = await work.Companies.UpdateAsync(entity);
            work.Save();

            affected.Should().Be(1);

            return entity;
        }

        private async Task<Company> RetrieveCompany(Guid id)
        {
            var result = await work.Companies.GetByIdAsync(id);
            work.Save();

            result.Should().NotBeNull();
            result.Id.Should().Equals(id);
            result.Name.Should().Equals("XXX");
            result.Description.Should().Equals("Description XXX");

            return result;
        }

        private async Task DeleteCompany(Company entity)
        {
            var affectedCompanies = await work.Companies.DeleteAsync(entity);
            work.Save();

            affectedCompanies.Should().Be(1);
        }
    }
}
