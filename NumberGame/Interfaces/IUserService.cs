using NumberGame.Model;
using System.Collections.Generic;

namespace NumberGame.Interfaces
{
    public interface IUserService
    {
        User GetCurrentUser();
        Response UserLogin();
        Response UpdateUserRecord(User user);
        List<User> GetLeaderBoard();
    }
}