using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using WebStoreElementLogic.Interfaces;
using System.Data.Common;


namespace WebStoreElementLogic.Data
{
    public class DapperService : IDapperService
    {
        public IDbConnection Connection { get; set; }

        private readonly IDbConnection _connection;
        private readonly IConfiguration _configuration;

        public DapperService(IConfiguration configuration)
        {
            _configuration = configuration;

            try
            {
                _connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
                _connection.Open();

                // checking if connection is succesfull
                if (_connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connection succesfull");
                }
                else
                {
                    Console.WriteLine("Connection failed");
                }
            }
            catch(Exception e) 
            {
                Console.WriteLine("No Connection: \n" + e.ToString());
            }
        }

        public T Get<T>(string sql, object value, CommandType commandType)
        {
            return _connection.QuerySingleOrDefault<T>(sql, value, commandType: commandType);
        }

        public List<T> GetAll<T>(string sql, object value, CommandType commandType)
        {
            return _connection.Query<T>(sql, value, commandType: commandType).ToList();
        }

        public T Insert<T>(string sql, DynamicParameters dbPara, CommandType commandType)
        {
            return _connection.ExecuteScalar<T>(sql, dbPara, commandType: commandType);
        }

        public int Execute(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Execute(sql, parameters, transaction, commandTimeout, commandType);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where TEntity : class
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                return await connection.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                return await connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        //public DbConnection Connection => (DbConnection)_connection;

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
