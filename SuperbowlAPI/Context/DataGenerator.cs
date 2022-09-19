using SuperbowlAPI.Models;
using System.Text.Json;

namespace SuperbowlAPI.Context
{
    public class DataGenerator
    {
        private readonly InMemoryContext _inMemoryContext;

        public DataGenerator(InMemoryContext inMemoryContext)
        {
            _inMemoryContext = inMemoryContext;
        }

        public void Generate()
        {
            if(!_inMemoryContext.GameModel.Any())
            {
                List<GameModel> items;
                using (var r = new StreamReader("SuperbowlDatabase.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<GameModel>>(json);
                }
                _inMemoryContext.GameModel.AddRange(items);
                _inMemoryContext.SaveChanges();
            }
            if(!_inMemoryContext.UserModel.Any())
            {
                var items = new List<UserModel>();
                var user = new UserModel();
                user.Name = "Anne Arendel";
                user.Password = "m1nh@s3nh@";
                user.Username = "usuario";
                user.Role = "Junior";
                items.Add(user);
                _inMemoryContext.UserModel.AddRange(items);
                _inMemoryContext.SaveChanges();
            }
        }
    }
}
