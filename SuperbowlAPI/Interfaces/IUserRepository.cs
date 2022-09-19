using SuperbowlAPI.Models;

namespace SuperbowlAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserModel>> Get(int page, int maxResults);
        Task<UserModel> Get(string username, string password);
       

        
    }
}
