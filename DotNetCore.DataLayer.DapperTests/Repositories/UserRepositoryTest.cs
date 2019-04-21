using DotNetCore.DataLayer.Dapper;
using DotNetCore.DataLayer.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCore.DataLayer.DapperTests.Repositories
{
    public class UserRepositoryTest
    {
        private readonly IDbFactory dbFactory = new NpgsqlDbFactory();
        private readonly IUnitOfWork work;

        private Company companyEntity = new Company
        {
            Name = "The Company",
            Description = "The Company Description",
        };
        

        public UserRepositoryTest()
        {
            work = new UnitOfWork(dbFactory);
        }

        [Fact]
        public async Task UserRepository_ShouldWork()
        {
            User userEntity = new User
            {
                Username = "jdoe",
                FirstName = "John",
                LastName = "Doe",
                Email = "jd@mail.com",
                Status = UserStatus.Pending
            };
            var user = await InsertUser(userEntity);
            user = await UpdateUser(user);
            user = await RetrieveUser(user.Id);
            await DeleteUser(user);
        }

        private async Task<User> InsertUser(User userEntity)
        {
            var companyResult = await work.Companies.InsertAsync(companyEntity);
            userEntity.CompanyId = companyResult.Id;
            var result = await work.Users.InsertAsync(userEntity);
            work.Save();

            return userEntity;
        }

        private async Task<User> UpdateUser(User userEntity)
        {
            userEntity.FirstName = "XXX";
            var affected = await work.Users.UpdateAsync(userEntity);
            work.Save();

            affected.Should().Be(1);

            return userEntity;
        }

        private async Task<User> RetrieveUser(Guid id)
        {
            var result = await work.Users.GetByIdAsync(id);
            work.Save();

            result.Should().NotBeNull();
            result.Id.Should().Equals(id);
            result.FirstName.Should().Equals("XXX");
            result.LastName.Should().Equals("Doe");
            result.Username.Should().Equals("jdoe");
            result.CompanyInfo.Id.Should().Equals(companyEntity.Id);
            result.CompanyInfo.Name.Should().Equals(companyEntity.Name);
            result.CompanyInfo.Description.Should().Equals(companyEntity.Description);

            return result;
        }

        private async Task DeleteUser(User userEntity)
        {

            var affectedUsers = await work.Users.DeleteAsync(userEntity);
            var affectedCompanies = await work.Companies.DeleteAsync(userEntity.CompanyInfo);
            work.Save();

            affectedUsers.Should().Be(1);
            affectedCompanies.Should().Be(1);

        }
    }
}
