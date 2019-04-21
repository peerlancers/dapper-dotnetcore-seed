using DotNetCore.DataLayer.Interfaces;
using System;

namespace DotNetCore.DataLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        ICompanyRepository Companies { get; }

        void Save();
    }
}
