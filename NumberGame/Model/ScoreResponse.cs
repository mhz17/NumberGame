using System;

namespace NumberGame.Model
{
    public class ScoreResponse: Response
    {
        public bool PlayAgain { get; set; }
        public int UserTotalScore { get; set; }
    }
}
