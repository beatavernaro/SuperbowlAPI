using SuperbowlAPI.Context;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Models;

namespace SuperbowlAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InMemoryContext _context;
        public UserRepository(InMemoryContext context)
        {
            _context = context;
        }
        public Task<List<UserModel>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var users = _context.UserModel.Skip((page - 1) * maxResults).Take(maxResults).ToList();
                return users.Any() ? users : new List<UserModel>();
            });
        }

        public Task<UserModel?> Get(string username, string password)
        {
            return Task.Run(() =>
            {
                var user = _context.UserModel.FirstOrDefault(item => item.Username.Equals(username) && item.Password.Equals(password));
                return user;
            });
        }
    }
}
