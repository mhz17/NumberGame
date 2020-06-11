using NumberGame.Enum;
using NumberGame.Interfaces;
using NumberGame.Model;

namespace NumberGame.Services
{
    public class ScoringService: IScoringService
    {

       private IConfigurationService _configurationService;

       public ScoringService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public int GetScore(NumberComparison actual, NumberComparison guess)
        {
            return actual == guess ? _configurationService.GetCorrectScore() : 0;
        }

        public ScoreResponse CompareScore(int score, int userTotalScore)
        {
            if (score == _configurationService.GetCorrectScore())
            {
                userTotalScore += 1;

                if (userTotalScore >= _configurationService.GetMaxTotalScore())
                {
                    return new ScoreResponse()
                    {
                        PlayAgain = false,
                        Success = true,
                        UserTotalScore = userTotalScore
                    };
                }
                else
                {
                    return new ScoreResponse()
                    {
                        PlayAgain = true,
                        Success = true,
                        UserTotalScore = userTotalScore
                    };
                }
            }
            else
            {
                return new ScoreResponse()
                {
                    PlayAgain = false,
                    Success = false,
                    UserTotalScore = userTotalScore
                };
            }
        }

    }
}
