using Dapper;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStoreElementLogic.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebStoreElementLogic.Data
{
    public class StockService : IStockService
    {
        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public StockService(IDapperService dapperService, IConfiguration configuration)
        {
            try
            {
                _configuration = configuration;
                _dapperService = dapperService;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not create DapperService");
                throw;
            }
        }

        // Method for updating the stock of a product in the database to 0 when the empty stock button is pressed.
        public async Task<int> EmptyStockAsync(int id)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@Id", id, DbType.Int64);
                result = _dapperService.Execute(
                                       "[dbo].[spUpdateStock]",
                                        dbPara,
                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error updating stock: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }
            return result;
        }

        public Task<int> EmptyStockAsync(Stock stock)
        {
            throw new NotImplementedException();
        }
    }
}