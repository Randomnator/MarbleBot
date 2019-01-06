using System;

namespace MarbleBot.Modules
{
    public class MoneyUser
    {
        public string Name { get; set; }
        public string Discriminator { get; set; }
        public decimal Balance { get; set; }
        public decimal NetWorth { get; set; }
        public uint DailyStreak { get; set; }
        public DateTime LastDaily { get; set; }
        public DateTime LastRaceWin { get; set;}
    }
}