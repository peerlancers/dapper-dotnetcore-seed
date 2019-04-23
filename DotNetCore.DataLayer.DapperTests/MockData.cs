using DotNetCore.DataLayer.Entities;
using System;

namespace DotNetCore.DataLayer.DapperTests
{
    public class MockData
    {
        public readonly Company Company = new Company
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
        };

        public readonly User User = new User
        { 
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            Status = UserStatus.Pending,
        };
    }
}
