using NumberGame.Interfaces;
using System;

namespace NumberGame.Services
{
    public class ConsoleLogService : IConsoleLogService
    {
        public string ReadInput()
        {
            return Console.ReadLine();
        }
        public void WriteOutput(string text)
        {
            Console.WriteLine(text);
        }

        public void CorrectResultOuput(int newNumber, int? points)
        {
            WriteOutput("");
            WriteOutput("**CORRECT**");
            WriteOutput($"Random Number: ---> {newNumber} <---");
            WriteOutput($"Total Score: {points}.");
        }

        public void InorrectResultOuput(int newNumber)
        {
            WriteOutput("");
            WriteOutput("**INCORRECT**");
            WriteOutput($"Random Number: ---> {newNumber} <---");
            WriteOutput("");
            WriteOutput($">> GAME OVER <<");
        }
    }
}
