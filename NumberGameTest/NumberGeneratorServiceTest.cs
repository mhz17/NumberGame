using NUnit.Framework;
using NumberGame.Services;
using System.Collections.Generic;
using NumberGame.Enum;
using NumberGame.Interfaces;
using Moq;

namespace NumberGameTest
{
    public class NumberGeneratorServiceTest
    {
        private Mock<IConfigurationService> _mockConfigurationService;

        [SetUp]
        public void TestSetup()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockConfigurationService.Setup(m => m.GetMinRandomNumber()).Returns(1);
            _mockConfigurationService.Setup(m => m.GetMaxRandomNumber()).Returns(100);
        }

        [Test]
        public void RequestRandomNumber_GeneratedNumberListEmpty_ReturnNumberBetween0And100()
        {
            var serviceUnderTest = new NumberGeneratorService(_mockConfigurationService.Object)
            {
                GeneratedNumbers = new List<int>()
            };

            var response = serviceUnderTest.GenerateRandomNumber();

            Assert.NotNull(response);
            Assert.GreaterOrEqual(response, 0);
            Assert.LessOrEqual(response, 100);
        }

        [Test]
        public void RequestRandomNumber_GeneratedNumberListNotEmpty_ReturnNumberBetween0And100ExludingGeneratedNumber()
        {
            var serviceUnderTest = new NumberGeneratorService(_mockConfigurationService.Object)
            {
                GeneratedNumbers = new List<int>()
                {
                    50, 20, 10, 15, 8, 0
                }
            };

            var response = serviceUnderTest.GenerateRandomNumber();

            Assert.NotNull(response);
            Assert.GreaterOrEqual(response, 0);
            Assert.LessOrEqual(response, 100);
            Assert.AreNotEqual(response, 50);
            Assert.AreNotEqual(response, 20);
            Assert.AreNotEqual(response, 10);
            Assert.AreNotEqual(response, 15);
            Assert.AreNotEqual(response, 8);
            Assert.AreNotEqual(response, 0);
        }

        [TestCase(10, 50, NumberComparison.Higher)]
        [TestCase(50, 50, NumberComparison.Higher)]
        [TestCase(50, 10, NumberComparison.Lower)]
        [TestCase(0, 10, NumberComparison.Higher)]
        public void CompareNumbers_HigherNumber_ReturnCorrectData(int previousNumber, int newNumber, NumberComparison expectedResult)
        {
            var serviceUnderTest = new NumberGeneratorService(_mockConfigurationService.Object);
            var result = serviceUnderTest.CompareNumbers(previousNumber, newNumber);
            Assert.AreEqual(expectedResult, result);
        }

    }
}