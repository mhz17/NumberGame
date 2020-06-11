using System;

namespace NumberGame.Model
{
    public class User
    {
        public string UserName { get; set; }
        public int? Points { get; set; }
        public TimeSpan? GameTime { get; set; }
    }
}
