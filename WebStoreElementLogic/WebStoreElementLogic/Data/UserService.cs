using Dapper;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStoreElementLogic.Entities;
using WebStoreElementLogic.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                dbPara.Add("@admin", user.admin, DbType.Boolean); // add this line
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
        public Task<User> GetUser(string userName)
        {
            var user = _dapperService.GetAll<User>($"SELECT * FROM [Users] WHERE userName = '{userName}'", null, commandType: CommandType.Text)
                                     .SingleOrDefault();
            return Task.FromResult(user);
        }


        //Med utgangspunkt i at vi ikke skal ha register på siden har jeg foreløpig ikke lagt til andre tasks her nå,
        //men det kan komme mer senere eller i en egen app for registrering.
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _dapperService.GetAllAsync<User>("SELECT * FROM [Users]");
            return users.ToList();
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@Id", id, DbType.Int32);
                result = await _dapperService.ExecuteAsync(
                    "[dbo].[spDeleteUser]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting user: " + ex.Message);
                throw;
            }

            return result;
        }

        // Getting the user by ID
        public Task<User> GetUserById(int id)
        {
            var user = _dapperService.Get<User>($"SELECT * FROM [Users] WHERE userId = @Id", new { Id = id }, commandType: CommandType.Text);
            return Task.FromResult(user);
        }


        public async Task<int> UpdateUserAsync(EditUserModel editUser)
        {
            int result = 0;
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@userId", editUser.userId, DbType.Int32);
                dbPara.Add("@userName", editUser.userName, DbType.String);
                dbPara.Add("@password", editUser.password, DbType.String);
                dbPara.Add("@admin", editUser.admin, DbType.Boolean);
                result = await _dapperService.ExecuteAsync(
                    "[dbo].[spUpdateUsers]",
                    dbPara,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user: " + ex.Message);
                throw;
            }

            return result;
        }

    }


}