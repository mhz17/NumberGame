using NUnit.Framework;
using NumberGame.Services;
using NumberGame.Enum;
using Moq;
using NumberGame.Interfaces;

namespace NumberGameTest
{
    public class ScoringServiceTest
    {

        private Mock<IConfigurationService> _mockConfigurationService;

        [SetUp]
        public void TestSetup()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
        }

        [TestCase(1, 10, NumberComparison.Higher, NumberComparison.Higher, 1)]
        [TestCase(1, 10, NumberComparison.Higher, NumberComparison.Lower, 0)]
        [TestCase(5, 10, NumberComparison.Higher, NumberComparison.Higher, 5)]
        [TestCase(5, 10, NumberComparison.Lower, NumberComparison.Higher, 0)]
        public void GetScore_CorrectScore_ReturnScore(
            int correctScore, 
            int maxScore, 
            NumberComparison compare_first, 
            NumberComparison compare_second,
            int resultScore)
        {
            _mockConfigurationService.Setup(a => a.GetCorrectScore()).Returns(correctScore);
            _mockConfigurationService.Setup(a => a.GetMaxTotalScore()).Returns(maxScore);

            var serviceUnderTest = new ScoringService(_mockConfigurationService.Object);

            var result = serviceUnderTest.GetScore(compare_first, compare_second);

            Assert.NotNull(result);
            Assert.GreaterOrEqual(resultScore, result);
        }

        [TestCase(1, 9, false, true, 10)]
        [TestCase(0, 9, false, false, 9)]
        [TestCase(1, 0, true, true, 1)]
        [TestCase(1, 4, true, true, 5)]
        public void CompareScore_ProvideScore_ReturnScoreResponse(
            int score,
            int totalScore,
            bool playAgain,
            bool success,
            int userScore)
        {
            _mockConfigurationService.Setup(a => a.GetCorrectScore()).Returns(1);
            _mockConfigurationService.Setup(a => a.GetMaxTotalScore()).Returns(10);

            var serviceUnderTest = new ScoringService(_mockConfigurationService.Object);
            var result = serviceUnderTest.CompareScore(score, totalScore);

            Assert.NotNull(result);
            Assert.GreaterOrEqual(playAgain, result.PlayAgain);
            Assert.GreaterOrEqual(success, result.Success);
            Assert.GreaterOrEqual(userScore, result.UserTotalScore);
        }
    }
}