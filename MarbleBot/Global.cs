﻿using System;
using System.Collections.Generic;

namespace MarbleBot
{
    class Global
    {
        /// <summary>
        /// Contains global variables
        /// </summary>

        internal static Random rand = new Random();
        internal static DateTime StartTime = new DateTime();

        // Server IDs
        internal const ulong CM = 223616088263491595; // Community Marble
        internal const ulong THS = 224277738608001024; // The Hat Stoar
        internal const ulong THSC = 318053169999511554; // The Hat Stoar Crew
        internal const ulong VFC = 394086559676235776; // Vinh Fan Club
        internal const ulong MT = 408694288604463114; // Melmon Test

        // Games
        internal static bool jumbleActive = false;
        internal static bool raceActive = false;
        internal static Dictionary<ulong, byte> Alive = new Dictionary<ulong, byte>();
    }
}
