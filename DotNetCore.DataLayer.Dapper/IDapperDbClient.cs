using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DotNetCore.DataLayer.Dapper
{
    public interface IDapperDbClient : IDbClient
    {
        Task<GridReader> QueryMultipleAsync(string sql, object param = null);
    }
}
