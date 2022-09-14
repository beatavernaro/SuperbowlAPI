using Microsoft.EntityFrameworkCore;
using SuperbowlAPI.Models;

namespace SuperbowlAPI.Context
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) :base(options)
        {

        }

        public DbSet<GameModel>? GameModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
