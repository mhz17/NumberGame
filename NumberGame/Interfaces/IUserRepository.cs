using NumberGame.Model;
using System.Collections.Generic;

namespace NumberGame.Interfaces
{
    public interface IUserRepository
    {
        User GetOrCreateUser(string username);
        void UpdateUser(User user);
        List<User> GetAllUsers();
    }
}