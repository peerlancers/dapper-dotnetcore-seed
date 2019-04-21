using System;

namespace DotNetCore.DataLayer.Entities
{
    public enum UserStatus
    {
        Pending,
        Active,
        Deactivated
    }

    public class User : Entity
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid? CompanyId { get; set; }

        public UserStatus Status { get; set; }

        public Company CompanyInfo { get; set; }
    }
}
