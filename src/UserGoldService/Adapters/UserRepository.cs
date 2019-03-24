using System;
using UserGoldService.Entities;

namespace UserGoldService.Adapters
{
    public interface IUserRepository
    {
        User Get(Guid id);
        void Update(User user);
        User GetByName(string name);
        void Create(User user);
    }

    public class UserRepository
    {
    }
}
