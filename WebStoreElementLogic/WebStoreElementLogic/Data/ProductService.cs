using Dapper;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Data
{
    public class ProductService : IProductService
    {
        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public ProductService(IDapperService dapperService, IConfiguration configuration)
        {
            try
            {
                _configuration = configuration;
                _dapperService = dapperService;
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not create DapperService");
                throw;
            }
        }

        public Task<int> Create(Product product)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@ExtProductId", product.Id, DbType.Int64);
                dbPara.Add("@ProductName", product.Name, DbType.String);
                dbPara.Add("@ProductDesc", product.Descr, DbType.String);
                dbPara.Add("@ImageId", product.URL, DbType.String);
                result = _dapperService.Execute(
                    "[dbo].[spAddProducts]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error creating product: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }

            return Task.FromResult(result);
        }

        public Task<Product> GetByID(int id)
        {
            var product = Task.FromResult(_dapperService.Get<Product>
                ($"SELECT * FROM [Products] WHERE ExtProductId = {id}",
                null, commandType: CommandType.Text));
            return product;
        }

        public Task<int> Delete(int id)
        {
            int deleteProduct = _dapperService.Execute
                ($"DELETE FROM [Products] WHERE ExtProductId = {id}",
                null, commandType: CommandType.Text);
            return Task.FromResult(deleteProduct);
        }

        public Task<int> Count(string search)
        {
            var totProduct = Task.FromResult(_dapperService.Get<int>
                ($"SELECT COUNT(*) FROM [Products] WHERE ProductName LIKE '%{search}%'",
                null, commandType: CommandType.Text));
            return totProduct;
        }

        public async Task UpdateQuantity(int productId, decimal quantity)
        {
            try
            {
                _dapperService.Execute
                    ($"UPDATE Stock SET Quantity = Quantity + {quantity} WHERE ExtProductId = {productId}",
                    null, commandType: CommandType.Text);
                Console.WriteLine($"Updated product {productId}'s quantity with {quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public Task<int> Update(Product product)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@ExtProductId", product.Id, DbType.Int64);
                dbPara.Add("@ProductName", product.Name, DbType.String);
                dbPara.Add("@ProductDesc", product.Descr, DbType.String);
                dbPara.Add("@ImageId", product.URL, DbType.String);
                result = _dapperService.Execute(
                    "[dbo].[spUpdateProducts]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error updating product: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }

            return Task.FromResult(result);
        }


        public async Task<List<Product>> ListAll(int page, int pageSize, string sortColumnName, string sortDirection, string searchTerm)
        {
            int offset = Math.Max(0, (page - 1) * pageSize); // Ensure the offset is not negative

            // Check if sort column name is provided, if not, set it to "ProductName"
            var sortClause = string.IsNullOrEmpty(sortColumnName) ? "Name" : sortColumnName;

            // Check if sort direction is provided, if not, set it to "ASC"
            sortClause += string.IsNullOrEmpty(sortDirection) ? " DESC" : " " + sortDirection;

            // Prepare the SQL statement
            var sql = $"SELECT * FROM [Products] WHERE ProductName LIKE @searchTerm";

            // Prepare the parameters to pass to the query
            var parameters = new { Offset = offset, PageSize = pageSize, SearchTerm = $"%{searchTerm}%" };

            // Execute the query and retrieve the results as a list of products
            var productsList = _dapperService.GetAll<Product>(sql, parameters, commandType: CommandType.Text);

            return productsList.ToList();
        }


        public Task<List<Product>> ListAllRefresh(string searchTerm, int page, int pageSize, string sortColumnName, string sortDirection)
        {
            int offset = Math.Max(0, (page - 1) * pageSize); // Ensure the offset is not negative

            var sortClause = string.IsNullOrEmpty(sortColumnName) ? "ProductName" : sortColumnName;
            sortClause += string.IsNullOrEmpty(sortDirection) ? " DESC" : " " + sortDirection;

            var sql = $"SELECT * FROM [Products] WHERE ProductName LIKE @searchTerm ORDER BY {sortClause} OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            var parameters = new { Offset = offset, PageSize = pageSize, SearchTerm = "%" + searchTerm + "%" };

            var productsList = _dapperService.GetAll<Product>(sql, parameters, commandType: CommandType.Text);
            return Task.FromResult(productsList.ToList());
        }

        // Task for showing product names from database
        public Task<List<Product>> GetProductNames(string Name)
        {
            var productsList = _dapperService.GetAll<Product>($"SELECT ProductName FROM [Products]", null, commandType: CommandType.Text);
            return Task.FromResult(productsList.ToList());
        }


        public Task<List<Product>> GetProduct(int Id)
        {
            var productsList = _dapperService.GetAll<Product>($"SELECT * FROM [Products] WHERE ExtProductId = {Id}", null, commandType: CommandType.Text);
            return Task.FromResult(productsList.ToList());
        }

        public Task<List<Product>> GetNextID(int Id)
        {
            var productsList = _dapperService.GetAll<Product>($"SELECT MAX(ExtProductId + 1) AS Id FROM [Products]", null, commandType: CommandType.Text);
            return Task.FromResult(productsList.ToList());
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProducts(string searchTerm, int pageIndex = 1, int pageSize = 10)
        {
            var sql = $"SELECT ExtProductId AS Id, ProductName AS Name, ProductDesc AS Descr, ImageId AS URL FROM [Products] WHERE ProductName LIKE @searchTerm ORDER BY ProductName";

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync();
                    var products = await connection.QueryAsync<Product>(sql, new { searchTerm = $"%{searchTerm}%" });
                    return products.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProductService, GetProducts: " + ex.Message);
                    return null;
                }
            }
        }





    }
}
