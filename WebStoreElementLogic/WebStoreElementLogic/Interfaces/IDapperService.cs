using Dapper;
using System.Data;

namespace WebStoreElementLogic.Interfaces
{
    public interface IDapperService
    {
        T Get<T>(string sql, object value, CommandType commandType);
        List<T> GetAll<T>(string sql, object value, CommandType commandType);
        T Insert<T>(string sql, DynamicParameters dbPara, CommandType commandType);
        int Execute(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        IDbConnection Connection { get; }
    }
}
