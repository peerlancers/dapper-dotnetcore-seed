using DotNetCore.DataLayer.DapperTests.TestOrdering;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCore.DataLayer.DapperTests.Repositories
{
    [TestCaseOrderer("DotNetCore.DataLayer.DapperTests.TestOrdering.PriorityOrderer", "DotNetCore.DataLayer.DapperTests")]
    public class CompanyRepositoryTest : RepositoryTestBase
    {
        private static readonly MockData mock = new MockData();

        [Fact, TestPriority(0)]
        public async Task Insert_ShouldWorkSuccessfully()
        {
            // Arrange
            var affected = await Work.Companies.InsertAsync(mock.Company);

            // Act
            Work.Save();

            // Assert
            affected.Should().Be(1, "entity should be added successfully");
        }

        [Fact, TestPriority(1)]
        public async Task Update_ShouldWorkSuccessfully()
        {
            // Arrange
            mock.Company.Name = Guid.NewGuid().ToString();
            mock.Company.Description = Guid.NewGuid().ToString();
            var affected = await Work.Companies.UpdateAsync(mock.Company);
            var record = await Work.Companies.GetByIdAsync(mock.Company.Id);

            // Act
            Work.Save();

            // Assert
            affected.Should().Be(1, "entity should be updated successfully");
            record.Should().NotBeNull("entity should be existing");
            record.Name.Should().Be(mock.Company.Name, "entity should have the updated Name");
            record.Description.Should().Be(mock.Company.Description, "entity should have the updated Description");
            record.LastUpdatedOn.Should().NotBeNull("entity should have the updated LastUpdatedOn");
        }

        [Fact, TestPriority(2)]
        public async Task GetById_ShouldWorkSuccessfully()
        {
            // Arrange
            var record = await Work.Companies.GetByIdAsync(mock.Company.Id);

            // Act
            Work.Save();

            // Assert
            record.Should().NotBeNull("entity should be existing");
            record.Id.Should().Be(mock.Company.Id);
            record.Name.Should().Be(mock.Company.Name, "entity should have the correct Name mapped");
            record.Description.Should().Be(mock.Company.Description, "entity should have the correct Description mapped");
        }

        [Fact, TestPriority(3)]
        public async Task Delete_ShouldWorkSuccessfully()
        {
            // Arrange
            var affectedCompanies = await Work.Companies.DeleteAsync(mock.Company);
            var record = await Work.Companies.GetByIdAsync(mock.Company.Id);

            // Act
            Work.Save();

            // Assert
            affectedCompanies.Should().Be(1, "entity should be deleted successfully");
            record.Should().BeNull("entity should no longer be existing");
        }
    }
}
