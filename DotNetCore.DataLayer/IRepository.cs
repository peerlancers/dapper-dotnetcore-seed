using System;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer
{
    public interface IRepository<TEntity> where TEntity : class, IDisposable, new()
    {
        void Dispose();

        Task<TEntity> InsertAsync(TEntity record);

        Task<int> DeleteAsync(TEntity record);

        Task<TEntity> GetByIdAsync(Guid id);

        Task<int> UpdateAsync(TEntity record);
    }
}
