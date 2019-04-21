using DotNetCore.DataLayer.Entities;
using System;

namespace DotNetCore.DataLayer.Interfaces
{
    public interface IUserRepository : IRepository<User>, IDisposable
    {
    }
}
