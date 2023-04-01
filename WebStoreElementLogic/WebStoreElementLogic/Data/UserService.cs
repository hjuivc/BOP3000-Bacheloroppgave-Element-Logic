using Dapper;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Data
{
    public class UserService : IUserService
    {
        private readonly IDapperService _dapperService;
        private readonly IConfiguration _configuration;

        public UserService(IDapperService dapperService, IConfiguration configuration)
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

        //For opprettelse av bruker
        public Task<int> Create(User user)
        {
            int result = 0;
            try
            {  
                var dbPara = new DynamicParameters();
                dbPara.Add("@userName", user.userName, DbType.String);
                dbPara.Add("@password", user.password, DbType.String);
                result = _dapperService.Execute(
                    "[dbo].[spAddUsers]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error creating user: " + ex.Message);
                // Re-throw the exception so the calling code can handle it
                throw;
            }

            return Task.FromResult(result);
        }

        //For at ny bruker får riktig ID
        public Task<List<User>> GetNextID(int userId)
        {
            var userList = _dapperService.GetAll<User>($"SELECT MAX(userId + 1) AS Id FROM [Users]", null, commandType: CommandType.Text);
            return Task.FromResult(userList.ToList());
        }

        //For å hente bruker og sammenligne hashet passord
        public Task<List<User>> GetUser(string userName)
        {
            {
                var userList = _dapperService.GetAll<User>($"SELECT * FROM [Users] WHERE userName = '{userName}'", null, commandType: CommandType.Text);
                return Task.FromResult(userList.ToList());

            }
        }
        //Med utgangspunkt i at vi ikke skal ha register på siden har jeg foreløpig ikke lagt til andre tasks her nå,
        //men det kan komme mer senere eller i en egen app for registrering.
    }
}
