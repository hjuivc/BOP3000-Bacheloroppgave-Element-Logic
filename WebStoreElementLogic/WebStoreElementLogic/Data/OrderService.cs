using Microsoft.Extensions.Configuration;
using WebStoreElementLogic.Entities;
using WebStoreElementLogic.Interfaces;
using Dapper;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStoreElementLogic.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebStoreElementLogic.Data
{
    public class OrderService : IOrderService {


        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public OrderService(IDapperService dapperService, IConfiguration configuration)
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

        // TODO: fix
        public async Task<int> GetNextID()
        {
            var orderList = _dapperService.GetAll<Product>($"SELECT MAX(ExtOrderId + 1) AS Id FROM [Order]", null, commandType: CommandType.Text);

            var productsList = await Task.FromResult(orderList.ToList());
            // Checks if the Id is null
            int newId = productsList?.FirstOrDefault()?.Id ?? 0;

            return Math.Max(newId, 1);
        }

        public async Task<int> GetNextPickListId()
        {
            var orderList = _dapperService.GetAll<Product>($"SELECT MAX(ExtPickListId + 1) AS Id FROM [Order]", null, commandType: CommandType.Text);

            var productsList = await Task.FromResult(orderList.ToList());
            // Checks if the Id is null
            int newId = productsList?.FirstOrDefault()?.Id ?? 0;

            return Math.Max(newId, 1);
        }

        // TODO: make getOne method in dapper service, and use for all get one cases like GetNextId, this etc.
        public async Task<int> GetNextTransactionId()
        {
            var sql = "SELECT NEXT VALUE FOR TransactionIdSequence";
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync();
                    var inbound = await connection.QuerySingleOrDefaultAsync<int>(sql);
                    return inbound;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        public async Task<int[]> Create(Dictionary<Product, int> CartDict)
        {
            int pickListId = await GetNextPickListId();
            int transactionId = await GetNextTransactionId();
            try
            {
                for (var i = 0;i < CartDict.Count; i++)
                {
                    var dbPara = new DynamicParameters();
                    dbPara.Add("@TransactionId", transactionId ,DbType.Int64);
                    dbPara.Add("@ExtPickListId", $"{pickListId}", DbType.String);
                    dbPara.Add("@ExtOrderLineId", i + 1, DbType.Int64);
                    dbPara.Add("@ExtProductId", CartDict.Keys.ElementAt(i).Id, DbType.Int64);
                    dbPara.Add("@Quantity", CartDict.Values.ElementAt(i), DbType.Decimal);

                    _dapperService.Execute(
                        "[dbo].[spAddOrder]",
                        dbPara,
                        commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error creating inbound: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }

            // Return to be used in eManager api call
            return new int[] { pickListId, transactionId };
        }

        /*
        public async Task<List<Inbound>> GetUnfinishedInbounds()
        {
            var sql = "SELECT InboundId, TransactionId, PurchaseOrderId, PurchaseOrderLineId, ExtProductId AS ProductId, Quantity, Status FROM Inbound WHERE Status = 0";
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync();
                    var inbounds = await connection.QueryAsync<Inbound>(sql);
                    return inbounds.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        */
        /*
        public async Task<Inbound> GetInbound(int transactionId)
        {
            var sql = "SELECT * FROM [Inbound] WHERE TransactionId = @TransactionId";
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync();
                    var inbound = await connection.QuerySingleOrDefaultAsync<Inbound>(sql);
                    return inbound;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        */
        public Task<string> Update(string ExtPickListId)
        {
            _dapperService.Execute
                ($"UPDATE Order SET Status = 1 WHERE ExtPickListId = {ExtPickListId}",
                null, commandType: CommandType.Text);

            Console.WriteLine($"Updated {ExtPickListId}");

            return Task.FromResult(ExtPickListId);
        }
    }
}
