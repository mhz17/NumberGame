using NumberGame.Interfaces;
using NumberGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumberGame.Services
{
    public class UserService: IUserService
    {

        private IUserRepository _userRepository;
        private IConsoleLogService _consoleLogService;
        public User currentUser { get; set; }
        public int loginCount { get; set; }

        public UserService(
            IUserRepository userRepository, 
            IConsoleLogService consoleLogService)
        {
            _userRepository = userRepository;
            _consoleLogService = consoleLogService;
        }

        private string RequestUserName()
        {
            _consoleLogService.WriteOutput("");
            _consoleLogService.WriteOutput("Please provide your username:");
            var username = _consoleLogService.ReadInput();

            while (!UserNameValid(username) && loginCount < 5)
            {
                _consoleLogService.WriteOutput("Invalid username.");
                loginCount += 1;
                return RequestUserName();
            }

            return username;

        }

        public Response UserLogin()
        {

            var username = RequestUserName();

            if (!UserNameValid(username))
            {
                return new Response()
                {
                    Success = false,
                    ConsoleOutput = $"Invalid username",
                    ErrorMessage = $"Exceeded login attempt"
                };
            }

            try
            {
                currentUser = _userRepository.GetOrCreateUser(username);
                currentUser.Points = 0;
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Success = false,
                    ConsoleOutput = $"Cannot create or get user record.",
                    ErrorMessage = ex.Message
                };
            }

            return new Response()
            {
                Success = true,
                ConsoleOutput = $"Welcome to the game: {currentUser.UserName}",
                ErrorMessage = null
            };

        }

        private bool UserNameValid(string username)
        {
            return (!string.IsNullOrEmpty(username) && Regex.IsMatch(username, "^[A-Za-z.\\s_-]+$"));
        }

        public Response UpdateUserRecord(User user)
        {

            if (!CheckIfRecordNeedsUpdating(user))
            {
                return new Response()
                {
                    Success = true,
                    ConsoleOutput = "Higher Score already exists for this user.",
                    ErrorMessage = null
                };
            }

            try
            {
                _userRepository.UpdateUser(user);
            }
            catch(Exception ex)
            {
                return new Response()
                {
                    Success = false,
                    ConsoleOutput = $"Record failed to save.",
                    ErrorMessage = ex.Message
                };
            }

            return new Response()
            {
                Success = true,
                ConsoleOutput = $"Record Saved.",
                ErrorMessage = null
            };

        }

        public List<User> GetLeaderBoard()
        {
            var users = _userRepository.GetAllUsers();
            return users.OrderByDescending(a => a.Points).ThenBy(t => t.GameTime).Take(3).ToList();
        }

        public User GetCurrentUser()
        {
            return currentUser;
        }

        private bool CheckIfRecordNeedsUpdating(User user)
        {
            var existingUser = _userRepository.GetOrCreateUser(user.UserName);
            if (existingUser == null)
            {
                return true;
            }
            else if(existingUser.Points < user.Points  || 
                existingUser.GameTime == null || 
                (existingUser.Points == user.Points && user.GameTime < existingUser.GameTime))
            {
                return true;
            }

            return false;
        }
    }
}
