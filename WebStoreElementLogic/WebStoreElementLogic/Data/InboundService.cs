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
    public class InboundService : IInboundService {


        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public InboundService(IDapperService dapperService, IConfiguration configuration)
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

        public async Task<int> GetNextID()
        {
            var inboundList = _dapperService.GetAll<Product>($"SELECT MAX(PurchaseOrderId + 1) AS Id FROM [Inbound]", null, commandType: CommandType.Text);

            var productsList = await Task.FromResult(inboundList.ToList());
            // Checks if the Id is null
            int newId = productsList?.FirstOrDefault()?.Id ?? 0;

            return Math.Max(newId, 1);
        }
        public async Task<int> Create(Product product, double qty)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@PurchaseOrderId", $"{await GetNextID()}", DbType.String);
                dbPara.Add("@ExtProductId", product.Id, DbType.Int64);
                dbPara.Add("@Quantity", qty, DbType.Decimal);
                result = _dapperService.Execute(
                    "[dbo].[spAddInbound]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error creating inbound: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }

            return result;
        }
        public Task<int> GetTransactionId(int nextPurchaseOrderId)
        {
            int transactionId = _dapperService.Execute
                ($"SELECT TransactionId FROM [Inbound] WHERE PurchaseOrderId = {nextPurchaseOrderId}",
                null, commandType: CommandType.Text);
            return Task.FromResult(transactionId);
        }

        public async Task<List<Inbound>> GetUnfinishedInbounds()
        {
            var sql = "SELECT I.*, P.ProductName AS Name FROM Inbound I JOIN Products P ON I.ExtProductId = P.ExtProductId WHERE I.Status = 0;";
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

        public Task<string> Update(string purchaseOrderId)
        {
            _dapperService.Execute
                ($"UPDATE Inbound SET Status = 1 WHERE PurchaseOrderId = {purchaseOrderId}",
                null, commandType: CommandType.Text);

            Console.WriteLine($"Updated {purchaseOrderId}");

            return Task.FromResult(purchaseOrderId);
        }

    }
}
