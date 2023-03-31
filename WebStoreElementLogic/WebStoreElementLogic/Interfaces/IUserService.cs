using WebStoreElementLogic.Entities;
namespace WebStoreElementLogic.Interfaces
{
    public interface IUserService
    {
        Task<int> Create(User user);
        Task<List<User>> GetNextID(int Id);
        Task<List<User>> GetUser(string UserName);



    }
}
