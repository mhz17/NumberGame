using NumberGame.Enum;
using NumberGame.Interfaces;
using System;
using System.Collections.Generic;

namespace NumberGame.Services
{
    public class NumberGeneratorService: INumberGeneratorService
    {
        private IConfigurationService _configurationService;
        public List<int> GeneratedNumbers;
        public int MaxNumber;

        public NumberGeneratorService(IConfigurationService configurationService)
        {
            GeneratedNumbers = new List<int>();
            _configurationService = configurationService;
            MaxNumber = 100;
        }

        public int GenerateRandomNumber()
        {
            Random random = new Random();

            int randomNumber = random.Next(_configurationService.GetMinRandomNumber(), _configurationService.GetMaxRandomNumber());
            if (GeneratedNumbers.Contains(randomNumber)){
                GenerateRandomNumber();
            }

            GeneratedNumbers.Add(randomNumber);

            return randomNumber;
        }

        public NumberComparison CompareNumbers(int previousNumber, int newNumber)
        {
            return newNumber >= previousNumber ? NumberComparison.Higher : NumberComparison.Lower;
        }

    }
}
