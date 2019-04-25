using DotNetCore.DataLayer.Dapper;
using DotNetCore.DataLayer.DapperTests.TestOrdering;
using DotNetCore.DataLayer.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCore.DataLayer.DapperTests.Repositories
{
    [TestCaseOrderer("DotNetCore.DataLayer.DapperTests.TestOrdering.PriorityOrderer", "DotNetCore.DataLayer.DapperTests")]
    public class UserRepositoryTest : RepositoryTestBase
    {
        private static MockData mock = new MockData();

        [Fact, TestPriority(0)]
        public async Task Insert_ShouldWorkSuccessfully()
        {
            // Arrange
            await Work.Companies.InsertAsync(mock.Company);
            mock.User.CompanyId = mock.Company.Id;
            mock.User.CompanyInfo = mock.Company;
            var affected = await Work.Users.InsertAsync(mock.User);

            // Act
            Work.Save();

            // Assert
            affected.Should().Be(1, "entity should be added successfully");
        }

        [Fact, TestPriority(1)]
        public async Task Update_ShouldWorkSuccessfully()
        {
            // Arrange
            mock.User.FirstName = Guid.NewGuid().ToString();
            mock.User.LastName = Guid.NewGuid().ToString();
            mock.User.Username = Guid.NewGuid().ToString();
            mock.User.Email = Guid.NewGuid().ToString();
            mock.User.Status = UserStatus.Active;
            var affected = await Work.Users.UpdateAsync(mock.User);
            var record = await Work.Users.GetByIdAsync(mock.User.Id);

            // Act
            Work.Save();

            // Assert
            affected.Should().Be(1, "entity should be updated successfully");
            record.Should().NotBeNull("entity should be existing");
            record.FirstName.Should().Be(mock.User.FirstName, "entity should have the updated FirstName");
            record.LastName.Should().Be(mock.User.LastName, "entity should have the updated LastName");
            record.Username.Should().Be(mock.User.Username, "entity should have the updated Username");
            record.Email.Should().Be(mock.User.Email, "entity should have the updated Email");
            record.Status.Should().Be(mock.User.Status, "entity should have the updated Status");
            record.LastUpdatedOn.Should().NotBeNull("entity should have the updated LastUpdatedOn");
        }

        [Fact, TestPriority(2)]
        public async Task GetById_ShouldWorkSuccessfully()
        {
            // Arrange
            var record = await Work.Users.GetByIdAsync(mock.User.Id);

            // Act
            Work.Save();

            // Assert
            record.Should().NotBeNull("entity should be existing");
            record.Id.Should().Be(mock.User.Id);
            record.FirstName.Should().Be(mock.User.FirstName, "entity should have the correct FirstName mapped");
            record.LastName.Should().Be(mock.User.LastName, "entity should have the correct LastName mapped");
            record.Username.Should().Be(mock.User.Username, "entity should have the correct Username mapped");
            record.Email.Should().Be(mock.User.Email, "entity should have the correct Email mapped");
            record.Status.Should().Be(mock.User.Status, "entity should have the correct Status mapped");
            record.CompanyId.Should().Be(mock.User.CompanyId, "entity should have the correct CompanyId mapped");
            record.CompanyInfo.Id.Should().Be(mock.Company.Id, "entity should have the correct CompanyInfo.Id mapped");
            record.CompanyInfo.Name.Should().Be(mock.Company.Name, "entity should have the correct CompanyInfo.Name mapped");
            record.CompanyInfo.Description.Should().Be(mock.Company.Description, "entity should have the correct CompanyInfo.Description mapped");
        }

        [Fact, TestPriority(3)]
        public async Task Delete_ShouldWorkSuccessfully()
        {
            // Arrange
            var affectedUsers = await Work.Users.DeleteAsync(mock.User);
            var record = await Work.Users.GetByIdAsync(mock.User.Id);

            // Act
            Work.Save();
            // Clean Up
            await Work.Companies.DeleteAsync(mock.Company);
            Work.Save();

            // Assert
            affectedUsers.Should().Be(1, "entity should be deleted successfully");
            record.Should().BeNull("entity should no longer be existing");

            
        }

        [Fact, TestPriority(4)]
        public async Task Search_ShouldWorkSuccessfully()
        {
            mock = new MockData();
            // Arrange
            await Work.Companies.InsertAsync(mock.Company);
            var user1 = new User()
            {
                Username = "jperez",
                FirstName = "John",
                LastName = "Perez",
                Email = "email2@gmail.com",
                Status = UserStatus.Pending,
                CompanyId = mock.Company.Id,
                CompanyInfo = mock.Company
            };
            var user2 = new User()
            {
                Username = "jregalado",
                FirstName = "John",
                LastName = "Regalado",
                Email = "email@gmail.com",
                Status = UserStatus.Active,
                CompanyId = mock.Company.Id,
                CompanyInfo = mock.Company
            };
            await Work.Users.InsertAsync(user1);
            await Work.Users.InsertAsync(user2);
            Work.Save();

            // Act
            var result = await Work.Users.SearchAsync(
                "jperez", 
                new Sorting<UserSortableFields>(UserSortableFields.LastName, SortDirection.Desc), 
                new PostgresPaging(1, 10));
            Work.Save();
            // Clean Up
            await Work.Users.DeleteAsync(user1);
            await Work.Users.DeleteAsync(user2);
            await Work.Companies.DeleteAsync(mock.Company);
            Work.Save();

            // Assert
            result.TotalCount.Should().Be(1);
            result.Records.Count.Should().Be(1);
            result.Records[0].Username.Should().Be("jperez", "entity should have the correct Username mapped");
            result.Records[0].FirstName.Should().Be("John", "entity should have the correct FirstName mapped");
            result.Records[0].LastName.Should().Be("Perez", "entity should have the correct LastName mapped");
            result.Records[0].Email.Should().Be("email2@gmail.com", "entity should have the correct Email mapped");
            result.Records[0].Status.Should().Be(UserStatus.Pending, "entity should have the correct Status mapped");
            result.Records[0].CompanyInfo.Id.Should().Be(mock.Company.Id, "entity should have the correct CompanyInfo.Id mapped");
            result.Records[0].CompanyInfo.Name.Should().Be(mock.Company.Name, "entity should have the correct CompanyInfo.Name mapped");
            result.Records[0].CompanyInfo.Description.Should().Be(mock.Company.Description, "entity should have the correct CompanyInfo.Description mapped");

            
        }
    }
}

