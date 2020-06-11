using NumberGame.Enum;

namespace NumberGame.Interfaces
{
    public interface INumberGeneratorService
    {
        NumberComparison CompareNumbers(int previousNumber, int newNumber);
        int GenerateRandomNumber();

    }
}