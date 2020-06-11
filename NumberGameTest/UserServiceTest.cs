using NUnit.Framework;
using NumberGame.Services;
using Moq;
using NumberGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using NumberGame.Interfaces;

namespace NumberGameTest
{
    public class UserServiceTest
    {

        private Mock<IUserRepository> _mockUserRespository;
        private Mock<IConsoleLogService> _mockConsoleLogService;

        [SetUp]
        public void TestSetup()
        {
            _mockUserRespository = new Mock<IUserRepository>();
            _mockConsoleLogService = new Mock<IConsoleLogService>();
        }

        private User CreateTestUser(string username, int points, TimeSpan time)
        {
            return new User
            {
                UserName = username,
                Points = points,
                GameTime = time
            };
        }

        [TestCase("123456")]
        [TestCase("")]
        [TestCase("?_&%^$")]
        public void UserLogin_UserNameEmptyOrInvalid_InvalidUserNameResponse(string username)
        {
            _mockConsoleLogService.Setup(a => a.ReadInput()).Returns(username);
            _mockConsoleLogService.Setup(a => a.WriteOutput("Please provide your username:"));

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.UserLogin();

            Assert.IsTrue(!response.Success);
            Assert.AreEqual(5, serviceUnderTest.loginCount);
            Assert.AreEqual("Invalid username", response.ConsoleOutput);
        }

        [Test]
        public void UserLogin_UserNameNotEmpty_ValidUserNameResponse()
        {

            var user = CreateTestUser("test", 0, new TimeSpan(100));
            _mockConsoleLogService.Setup(a => a.ReadInput()).Returns(user.UserName);
            _mockConsoleLogService.Setup(a => a.WriteOutput("Please provide your username:"));
            _mockUserRespository.Setup(m => m.GetOrCreateUser(user.UserName)).Returns(user);

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.UserLogin();

            Assert.IsTrue(response.Success);
            Assert.AreEqual($"Welcome to the game: {user.UserName}", response.ConsoleOutput);
        }

        [Test]
        public void UpdateUserRecord_UserNotEmpty_SuccessResponse()
        {
            var user = CreateTestUser("test", 0, new TimeSpan(100));
            _mockConsoleLogService.Setup(a => a.ReadInput()).Returns(user.UserName);
            _mockConsoleLogService.Setup(a => a.WriteOutput("Please provide your username:"));
            _mockUserRespository.Setup(m => m.UpdateUser(user));

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.UpdateUserRecord(user);

            Assert.IsTrue(response.Success);
        }


        [Test]
        public void UpdateUserRecord_UserEmpty_ErrorResponse()
        {
            var user = CreateTestUser("test", 0, new TimeSpan(100));
            _mockConsoleLogService.Setup(a => a.ReadInput()).Returns(user.UserName);
            _mockConsoleLogService.Setup(a => a.WriteOutput("Please provide your username:"));
            _mockUserRespository.Setup(m => m.UpdateUser(user)).Throws(new Exception("Test Error Message"));

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.UpdateUserRecord(user);

            Assert.IsTrue(!response.Success);
            Assert.AreEqual("Test Error Message", response.ErrorMessage);
        }

        [TestCase(5, 10, "00:00:30", "00:00:30", true, "Record Saved.")]
        [TestCase(0, 1, "00:00:35", "00:00:20", true, "Record Saved.")]
        [TestCase(10, 5, "00:00:50", "00:00:30", true, "Higher Score already exists for this user.")]
        [TestCase(5, 5, "00:00:30", "00:00:30", true, "Higher Score already exists for this user.")]
        [TestCase(5, 5, "00:00:25", "00:00:30", true, "Higher Score already exists for this user.")]
        [TestCase(5, 5, "00:00:25", "00:00:20", true, "Record Saved.")]
        public void UpdateUserRecord_UserHasHigherPreviousScore_SuccessResponseButNoUpdate(
            int currentScore, 
            int newScore, 
            TimeSpan currentTimeSpan, 
            TimeSpan newTimeSpan, 
            bool success,
            string consoleOutput)
        {
            var newUser = CreateTestUser("test", newScore, newTimeSpan);
            var existingUser = CreateTestUser("test", currentScore, currentTimeSpan);

            _mockConsoleLogService.Setup(a => a.ReadInput()).Returns(newUser.UserName);
            _mockConsoleLogService.Setup(a => a.WriteOutput("Please provide your username:"));
            _mockUserRespository.Setup(m => m.GetOrCreateUser(existingUser.UserName)).Returns(existingUser);

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.UpdateUserRecord(newUser);

            Assert.AreEqual(success, response.Success);
            Assert.AreEqual(consoleOutput, response.ConsoleOutput);
        }

        [Test]
        public void GetLeaderBoard_MoreThan3Users_ReturnTop3Users()
        {

            var users = new List<User>
            {
                new User()
                {
                    UserName = "test1",
                    Points = 5,
                    GameTime = new TimeSpan(100)
                },
                new User()
                {
                    UserName = "test2",
                    Points = 4,
                    GameTime = new TimeSpan(100)
                },
                new User()
                {
                    UserName = "test3",
                    Points = 8,
                    GameTime = new TimeSpan(100)
                },
                new User()
                {
                    UserName = "test4",
                    Points = 9,
                    GameTime = new TimeSpan(100)
                },
                new User()
                {
                    UserName = "test5",
                    Points = 1,
                    GameTime = new TimeSpan(100)
                },
                new User()
                {
                    UserName = "test6",
                    Points = 9,
                    GameTime = new TimeSpan(101)
                }
            };

            _mockUserRespository.Setup(m => m.GetAllUsers()).Returns(users);

            var serviceUnderTest = new UserService(_mockUserRespository.Object, _mockConsoleLogService.Object);

            var response = serviceUnderTest.GetLeaderBoard();

            Assert.AreEqual(3, response.Count);
            Assert.AreEqual("test4", response.First().UserName);
            Assert.AreEqual("test6", response.Take(2).Last().UserName);
            Assert.AreEqual("test3", response.Take(3).Last().UserName);
        }

    }
}