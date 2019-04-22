using DotNetCore.DataLayer.Attributes;
using System;

namespace DotNetCore.DataLayer.Entities
{
    public enum UserStatus
    {
        Pending,
        Active,
        Deactivated
    }

    [DbTableName("users")]
    public class User : Entity
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid? CompanyId { get; set; }

        public UserStatus Status { get; set; }

        [DbIgnore]
        public Company CompanyInfo { get; set; }
    }
}
