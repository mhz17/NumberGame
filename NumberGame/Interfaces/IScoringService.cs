using NumberGame.Enum;
using NumberGame.Model;

namespace NumberGame.Interfaces
{
    public interface IScoringService
    {
        int GetScore(NumberComparison actual, NumberComparison guess);
        ScoreResponse CompareScore(int score, int userTotalScore);
    }
}