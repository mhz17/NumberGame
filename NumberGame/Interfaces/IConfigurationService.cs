
namespace NumberGame.Interfaces
{
    public interface IConfigurationService
    {
        string GetFilePath();
        int GetMaxTotalScore();
        int GetCorrectScore();
        int GetMinRandomNumber();
        int GetMaxRandomNumber();
    }
}