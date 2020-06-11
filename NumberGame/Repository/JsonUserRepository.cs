using Newtonsoft.Json;
using NumberGame.Interfaces;
using NumberGame.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NumberGame.Repository
{
    public class JsonUserRepository : IUserRepository
    {

        private IConfigurationService _configurationService;

        public JsonUserRepository(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public User GetOrCreateUser(string username)
        {
            var users = ReadFile();
            if (users != null)
            {
                var currentUser = users.Where(u => u.UserName == username).FirstOrDefault();
                if (currentUser != null)
                {
                    return currentUser;
                }
            }
            else
            {
                users = new List<User>();
            }

            return AddUser(users, username);

        }

        private List<User> ReadFile()
        {
            using StreamReader r = new StreamReader(_configurationService.GetFilePath());
            string json = r.ReadToEnd();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        } 

        private void SaveFile(List<User> users)
        {
            var str = JsonConvert.SerializeObject(users);
            using StreamWriter w = new StreamWriter(_configurationService.GetFilePath());
            w.Write(str);
        }

        private User AddUser(List<User> users, string username)
        {
            var newUser = new User { UserName = username, Points = 0, GameTime = null };
            users.Add(newUser);
            SaveFile(users);
            return newUser;
        }

        public void UpdateUser(User user)
        {
            var users = ReadFile();
            if (users != null)
            {
                var currentUser = users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                if (currentUser != null)
                {
                    currentUser.Points = user.Points;
                    currentUser.GameTime = user.GameTime;
                    SaveFile(users);
                }
            }
        }

        public List<User> GetAllUsers()
        {
            var users = ReadFile();
            return users;
        }
    }

}
