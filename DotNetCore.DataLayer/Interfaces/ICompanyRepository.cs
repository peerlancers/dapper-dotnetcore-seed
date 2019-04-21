using DotNetCore.DataLayer.Entities;
using System;

namespace DotNetCore.DataLayer.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>, IDisposable
    {
    }
}
