using NumberGame.Interfaces;
using System;
using System.Configuration;
using System.IO;

namespace NumberGame.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private const string CorrectScore = "CorrectScore";
        private const string MaxTotalScore = "MaxTotalScore";
        private const string FilePath = "FilePath";
        private const string MinRandomNumber = "MinRandomNumber";
        private const string MaxRandomNumber = "MaxRandomNumber";

        public int GetCorrectScore()
        {
            return int.Parse(ConfigurationManager.AppSettings[CorrectScore]);
        }

        public string GetFilePath()
        {
            var parentOfStartupPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../../"));
            return Path.Combine(parentOfStartupPath, ConfigurationManager.AppSettings[FilePath]);
        }

        public int GetMaxTotalScore()
        {
            return int.Parse(ConfigurationManager.AppSettings[MaxTotalScore]);
        }

        public int GetMaxRandomNumber()
        {
            return int.Parse(ConfigurationManager.AppSettings[MaxRandomNumber]);
        }

        public int GetMinRandomNumber()
        {
            return int.Parse(ConfigurationManager.AppSettings[MinRandomNumber]);
        }
    }
}
