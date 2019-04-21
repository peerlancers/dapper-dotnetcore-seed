using System;
using System.Data;

namespace DotNetCore.DataLayer
{
    public interface IDbFactory : IDisposable
    {
        IDbConnection Context();
    }
}
