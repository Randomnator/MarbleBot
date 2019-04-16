﻿using Discord.Commands;
using System;

namespace MarbleBot.BaseClasses
{
    /// <summary> Marble in a Siege. </summary>
    public class Marble
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int DamageDealt { get; set; }
        public int PUHits { get; set; }
        public byte ItemAccuracy { get; set; } = 100;
        public bool Cloned { get; set; }
        public bool QefpedunCharmUsed { get; set; }
        public MSE StatusEffect { get; set; }
        public DateTime DoomStart { get; set; }
        public DateTime LastPoisonTick { get; set; }
        public DateTime LastStun { get; set; }

        public Marble()
        {
            DoomStart = DateTime.Parse("2019-01-01 00:00:00");
            LastStun = DateTime.Parse("2019-01-01 00:00:00");
            LastPoisonTick = DateTime.Parse("2019-01-01 00:00:00");
        }

        public override string ToString() => $"{Name} HP: {HP}/20 [{Id}] BH: {DamageDealt} PH: {PUHits} Cloned: {Cloned}";

        public string ToString(SocketCommandContext context)
        {
            var user = context.Client.GetUser(Id);
            return $"**{Name}** (HP: **{HP}**/{MaxHP}, DMG: **{DamageDealt}**) [{user.Username}#{user.Discriminator}]";
        }
    }
}
