using NumberGame.Enum;
using NumberGame.Services;
using System;
using Microsoft.Extensions.DependencyInjection;
using NumberGame.Interfaces;
using NumberGame.Repository;

namespace NumberGame
{
    class Program
    {
        private static IConsoleLogService _consoleLogService;
        private static IUserService _userService;
        private static INumberGeneratorService _numberGeneratorService;
        private static IScoringService _scoringService;

        private static DateTime _startTime;
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IConfigurationService, ConfigurationService>()
                .AddTransient<IConsoleLogService, ConsoleLogService>()
                .AddTransient<IUserRepository, JsonUserRepository>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<INumberGeneratorService, NumberGeneratorService>()
                .AddTransient<IScoringService, ScoringService>()
                .BuildServiceProvider();

            _consoleLogService = serviceProvider.GetService<IConsoleLogService>();
            _userService = serviceProvider.GetService<IUserService>();
            _numberGeneratorService = serviceProvider.GetService<INumberGeneratorService>();
            _scoringService = serviceProvider.GetService<IScoringService>();

            _consoleLogService.WriteOutput("**** WELCOME TO NUMBER GAME *****");
            _consoleLogService.WriteOutput("");

            var response = _userService.UserLogin();
            _consoleLogService.WriteOutput("");
            _consoleLogService.WriteOutput(response.ConsoleOutput);

            if (response.Success)
            {
                _startTime = DateTime.Now;
                PlayGame();
            }
            else
            {
                _consoleLogService.WriteOutput("");
                _consoleLogService.WriteOutput(">> ERROR <<");
                _consoleLogService.WriteOutput(response.ConsoleOutput);
            }

            PrintLeaderBoard();
            _consoleLogService.ReadInput();
        }

        public static void PlayGame()
        {
            _consoleLogService.WriteOutput("");
            _consoleLogService.WriteOutput("*************************");

            var number = _numberGeneratorService.GenerateRandomNumber();
            _consoleLogService.WriteOutput($"Random Number is: ---> {number} <---");
            _consoleLogService.WriteOutput("Is the next number higher (H) or lower (any other key)?");

            var guess = _consoleLogService.ReadInput();
            var newNumber = _numberGeneratorService.GenerateRandomNumber();
            var result = _numberGeneratorService.CompareNumbers(number, newNumber);
            var score = _scoringService.GetScore(result, guess.ToLower() == "h" ? NumberComparison.Higher : NumberComparison.Lower);

            var scoringResponse = _scoringService.CompareScore(score, _userService.GetCurrentUser().Points == null ? 0 : _userService.GetCurrentUser().Points.Value);
            _userService.GetCurrentUser().Points = scoringResponse.UserTotalScore;

            if (scoringResponse.Success)
            {
                if (!scoringResponse.PlayAgain)
                {
                    _consoleLogService.CorrectResultOuput(newNumber, _userService.GetCurrentUser().Points);
                    _consoleLogService.WriteOutput("You achieved max points");
                    _userService.GetCurrentUser().GameTime = DateTime.Now - _startTime;
                    _userService.UpdateUserRecord(_userService.GetCurrentUser());

                    if (PlayAgain())
                    {
                        PlayGame();
                    }
                }
                else
                {
                    _consoleLogService.CorrectResultOuput(newNumber, _userService.GetCurrentUser().Points);
                    _consoleLogService.WriteOutput("Try again");
                    PlayGame();
                }
            }
            else
            {
                _consoleLogService.InorrectResultOuput(newNumber);
                _userService.GetCurrentUser().GameTime = DateTime.Now - _startTime;
                _userService.UpdateUserRecord(_userService.GetCurrentUser());
                if (PlayAgain())
                {
                    PlayGame();
                }
            }
  
        }

        public static bool PlayAgain()
        {
            _consoleLogService.WriteOutput("");
            _consoleLogService.WriteOutput("Do you want to play again? Y (y) or N (any other key)");
            var answer = _consoleLogService.ReadInput();
            return answer.ToLower() == "y";
        }

        public static void PrintLeaderBoard()
        {
            _consoleLogService.WriteOutput("");
            var bestUsers = _userService.GetLeaderBoard();
            _consoleLogService.WriteOutput("*********LEADER BOARD (TOP 3)*********");
            _consoleLogService.WriteOutput("");
            bestUsers.ForEach(u =>
            {
                _consoleLogService.WriteOutput($"{u.UserName}\t{u.Points}\t{u.GameTime}");
            });
            _consoleLogService.WriteOutput("");
            _consoleLogService.WriteOutput("******************************");
        }
    }
}
