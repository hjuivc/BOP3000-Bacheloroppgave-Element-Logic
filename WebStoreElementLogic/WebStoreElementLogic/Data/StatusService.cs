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
    public class StatusService : IStatusService {
        

        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public StatusService(IDapperService dapperService, IConfiguration configuration)
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

        public async Task<List<Order>> GetUnfinishedOrders()
        {
            var sql = "SELECT Order.*, P.ProductName AS Name FROM Order JOIN Products P ON Order.ExtProductId = P.ExtProductId WHERE Order.Status = 0;";
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync();
                    var orders = await connection.QueryAsync<Order>(sql);
                    return orders.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

    }
}
    
