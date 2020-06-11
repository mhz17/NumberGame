namespace NumberGame.Interfaces
{
    public interface IConsoleLogService
    {
        string ReadInput();
        void WriteOutput(string text);
        void CorrectResultOuput(int newNumber, int? points);
        void InorrectResultOuput(int newNumber);
    }
}