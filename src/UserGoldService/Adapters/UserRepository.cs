using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserGoldService.Entities;

namespace UserGoldService.Adapters
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);
        Task Update(User user);
        Task<User> GetByName(string name);
        Task Create(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly GoldServiceContext _db;
        public UserRepository(GoldServiceContext db)
        {
            _db = db;
        }

        public Task Create(User user)
        {
            _db.Users.Add(user);
            return _db.SaveChangesAsync();
        }

        public Task<User> Get(Guid id)
        {
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.id == id);
        }

        public Task<User> GetByName(string name)
        {
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Name == name);
        }

        public Task Update(User user)
        {
            _db.Users.Update(user);
            return _db.SaveChangesAsync();
        }
    }
}
