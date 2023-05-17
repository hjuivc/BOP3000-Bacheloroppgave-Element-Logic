using WebStoreElementLogic.Entities;
namespace WebStoreElementLogic.Interfaces
{
    public interface IUserService
    {
        Task<int> Create(User user);
        Task<List<User>> GetNextID(int userId);
        Task<User> GetUser(string userName);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserById(int id);
        Task<int> DeleteUserAsync(int id);
        Task<int> UpdateUserAsync(EditUserModel editUser);

    }
}