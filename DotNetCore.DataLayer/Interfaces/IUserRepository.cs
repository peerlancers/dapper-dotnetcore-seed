using DotNetCore.DataLayer.Entities;
using System;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer.Interfaces
{
    public interface IUserRepository : IRepository<User>, IDisposable
    {
        Task<DbRecordList<User>> SearchAsync(string keyword, Sorting<UserSortableFields> sorting = null, IPagingInfo paging = null);
    }
}
