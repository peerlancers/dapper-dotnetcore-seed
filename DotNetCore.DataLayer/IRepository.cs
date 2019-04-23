using System;
using System.Threading.Tasks;

namespace DotNetCore.DataLayer
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, IDisposable, new()
    {
        void Dispose();

        Task<int> InsertAsync(TEntity record);

        Task<TEntity> GetByIdAsync(Guid id);

        Task<int> DeleteAsync(TEntity record);

        Task<int> UpdateAsync(TEntity record);
    }
}
