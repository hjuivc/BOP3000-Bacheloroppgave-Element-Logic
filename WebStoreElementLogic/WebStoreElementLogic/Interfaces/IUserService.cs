using WebStoreElementLogic.Entities;
namespace WebStoreElementLogic.Interfaces
{
    public interface IUserService
    {
        Task<int> Create(User user);
        Task<List<User>> GetNextID(int userId);
        Task<List<User>> GetUser(string userName);
    }
}
