

using Microsoft.EntityFrameworkCore;
using UserGoldService.Entities;

namespace UserGoldService.Adapters
{
    public class GoldServiceContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public GoldServiceContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=GoldService.db");
        }
    }
}
