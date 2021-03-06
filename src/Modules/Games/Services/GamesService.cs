﻿using MarbleBot.Common.Games.Scavenge;
using MarbleBot.Common.Games.Siege;
using MarbleBot.Common.Games.War;
using System.Collections.Concurrent;

namespace MarbleBot.Modules.Games.Services
{
    public class GamesService
    {
        public ConcurrentDictionary<ulong, Scavenge> Scavenges { get; set; } = new();

        public ConcurrentDictionary<ulong, Siege> Sieges { get; set; } = new();
        public ConcurrentDictionary<ulong, War> Wars { get; set; } = new();
    }
}
