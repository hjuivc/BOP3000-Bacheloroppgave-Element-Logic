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

        private readonly string _connectionString = "Server=AASS-PC\\SQLEXPRESS01;Database=Element Logic (Web Shop);Persist Security Info=False;User Id=admin;Password=usnADMIN2020;Encrypt=false";
        private readonly IDbConnection _connection;

        public DapperService()
        {
            _connection = new SqlConnection(_connectionString);
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

        //public DbConnection Connection => (DbConnection)_connection;

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
