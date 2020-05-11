﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MarbleBot.Common;
using MarbleBot.Extensions;
using MarbleBot.Modules.Games.Services;
using MarbleBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MarbleBot.Modules.Games
{
    public class GameModule : MarbleBotModule
    {
        protected BotCredentials _botCredentials;
        protected GamesService _gamesService;
        protected RandomService _randomService;

        public GameModule(BotCredentials botCredentials, GamesService gamesService, RandomService randomService)
        {
            _botCredentials = botCredentials;
            _gamesService = gamesService;
            _randomService = randomService;
        }

        private string GameName(GameType gameType, bool capitalised = true)
        => capitalised ? Enum.GetName(typeof(GameType), gameType)! : Enum.GetName(typeof(GameType), gameType)!.ToLower();

        protected async Task Checkearn(GameType gameType)
        {
            var user = MarbleBotUser.Find(Context);
            var lastWin = gameType switch
            {
                GameType.Race => user.LastRaceWin,
                GameType.Siege => user.LastSiegeWin,
                GameType.War => user.LastWarWin,
                _ => user.LastScavenge,
            };
            var nextEarn = (DateTime.UtcNow - lastWin);
            string game = gameType switch
            {
                GameType.Race => "race",
                GameType.Scavenge => "scavenge",
                GameType.Siege => "siege",
                GameType.War => "war",
                _ => "none"
            };
            var output = nextEarn.TotalHours < 6 ?
                $"You can earn money from {game} in **{GetTimeSpanSentence(lastWin - DateTime.UtcNow.AddHours(-6))}**!"
                : $"You can earn money from {game} now!";
            await ReplyAsync(embed: new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithColor(GetColor(Context))
                .WithCurrentTimestamp()
                .WithDescription(output)
                .Build());
        }

        protected async Task Clear(GameType gameType)
        {
            ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
            if (_botCredentials.AdminIds.Any(id => id == Context.User.Id) || Context.IsPrivate)
            {
                using var marbleList = new StreamWriter($"Data{Path.DirectorySeparatorChar}{fileId}.{GameName(gameType, false)}", false);
                await marbleList.WriteAsync("");
                await ReplyAsync("Contestant list successfully cleared!");
            }
            else
            {
                await ReplyAsync($"**{Context.User.Username}**, you cannot do this!");
            }
        }

        protected static string Leaderboard(IEnumerable<(string elementName, int value)> orderedData, int no)
        {
            var dataList = new List<(int place, string elementName, int value)>();
            int displayedPlace = 0, lastValue = 0;
            foreach (var (elementName, value) in orderedData)
            {
                if (value != lastValue)
                {
                    displayedPlace++;
                }

                dataList.Add((displayedPlace, elementName, value));
                lastValue = value;
            }
            if (no > dataList.Last().place / 10)
            {
                return $"There are no entries in page **{no}**!";
            }
            // This displays in groups of ten (i.e. if no is 1, first 10 displayed;
            // no = 2, next 10, etc.
            int minValue = (no - 1) * 10 + 1, maxValue = no * 10;
            var output = new StringBuilder();
            foreach (var (place, elementName, value) in dataList)
            {
                if (place > maxValue)
                {
                    break;
                }

                if (place < maxValue + 1 && place >= minValue)
                {
                    output.AppendLine($"{place}{place.Ordinal()}: {elementName} {value}");
                }
            }
            if (output.Length > 2048)
            {
                return string.Concat(output.ToString().Take(2048));
            }

            return output.ToString();
        }

        protected async Task RemoveContestant(GameType gameType, string marbleToRemove)
        {
            ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
            string marbleListDirectory = $"Data{Path.DirectorySeparatorChar}{fileId}.{GameName(gameType, false)}";
            if (!File.Exists(marbleListDirectory))
            {
                await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                return;
            }

            // 0 - Not found, 1 - Found but not yours, 2 - Found & removed
            int state = _botCredentials.AdminIds.Any(id => id == Context.User.Id) ? 3 : 0;
            var wholeFile = new StringBuilder();
            var formatter = new BinaryFormatter();
            if (gameType == GameType.War)
            {
                List<(ulong id, string name, int itemId)> marbles;
                using (var marbleListFile = new StreamReader(marbleListDirectory))
                {
                    if (marbleListFile.BaseStream.Length == 0)
                    {
                        await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                        return;
                    }
                    marbles = (List<(ulong id, string name, int itemId)>)formatter.Deserialize(marbleListFile.BaseStream);
                }

                if (marbles.Any(info => string.Compare(marbleToRemove, info.name, true) == 0))
                {
                    var (id, name, itemId) = marbles.Find(info => string.Compare(marbleToRemove, info.name, true) == 0);
                    if (state == 2 || id == Context.User.Id)
                    {
                        state = 2;
                        marbles.Remove((id, name, itemId));
                    }
                    else
                    {
                        state = 1;
                    }
                }

                using (var marbleListFile = new StreamWriter(marbleListDirectory))
                {
                    if (marbleListFile.BaseStream.Length == 0)
                    {
                        await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                        return;
                    }
                    formatter.Serialize(marbleListFile.BaseStream, marbles);
                }
            }
            else
            {
                List<(ulong id, string name)> marbles;
                using (var marbleListFile = new StreamReader(marbleListDirectory))
                {
                    marbles = (List<(ulong id, string name)>)formatter.Deserialize(marbleListFile.BaseStream);
                }

                if (marbles.Any(info => string.Compare(marbleToRemove, info.name, true) == 0))
                {
                    var (id, name) = marbles.Find(info => string.Compare(marbleToRemove, info.name, true) == 0);
                    if (state == 2 || id == Context.User.Id)
                    {
                        state = 2;
                        marbles.Remove((id, name));
                    }
                    else
                    {
                        state = 1;
                    }
                }

                using (var marbleListFile = new StreamWriter(marbleListDirectory))
                {
                    formatter.Serialize(marbleListFile.BaseStream, marbles);
                }
            }

            switch (state)
            {
                case 0: await ReplyAsync($"**{Context.User.Username}**, could not find the requested marble!"); break;
                case 1: await ReplyAsync($"**{Context.User.Username}**, this is not your marble!"); break;
                case 2:
                    string bold = marbleToRemove.Contains('*') || marbleToRemove.Contains('\\') ? "" : "**";
                    using (var marbleList = new StreamWriter(marbleListDirectory, false))
                    {
                        await marbleList.WriteAsync(wholeFile.ToString());
                        await ReplyAsync($"**{Context.User.Username}**, removed contestant {bold}{marbleToRemove}{bold}!");
                    }
                    break;
            }
        }

        protected async Task ShowContestants(GameType gameType)
        {
            ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
            string marbleListDirectory = $"Data{Path.DirectorySeparatorChar}{fileId}.{GameName(gameType, false)}";
            if (!File.Exists(marbleListDirectory))
            {
                await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                return;
            }

            var marbleOutput = new StringBuilder();
            int count = 0;
            using (var marbleListFile = new StreamReader(marbleListDirectory))
            {
                if (marbleListFile.BaseStream.Length == 0)
                {
                    await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                    return;
                }

                var formatter = new BinaryFormatter();
                string bold;
                SocketUser user;
                if (gameType == GameType.War)
                {
                    var marbles = (List<(ulong id, string name, int itemId)>)formatter.Deserialize(marbleListFile.BaseStream);
                    count = marbles.Count;
                    if (count == 0)
                    {
                        await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                        return;
                    }
                    foreach (var (id, name, itemId) in marbles)
                    {
                        bold = name.Contains('*') || name.Contains('\\') ? "" : "**";
                        user = Context.Client.GetUser(id);
                        marbleOutput.AppendLine($"{bold}{name}{bold} (Weapon: **{Item.Find<Item>(itemId.ToString()).Name}**) [{user.Username}#{user.Discriminator}]");
                    }
                }
                else
                {
                    var marbles = (List<(ulong id, string name)>)formatter.Deserialize(marbleListFile.BaseStream);
                    count = marbles.Count;
                    if (count == 0)
                    {
                        await SendErrorAsync($"**{Context.User.Username}**, no-one is signed up!");
                        return;
                    }
                    foreach (var (id, name) in marbles)
                    {
                        bold = name.Contains('*') || name.Contains('\\') ? "" : "**";
                        user = Context.Client.GetUser(id);
                        marbleOutput.AppendLine($"{bold}{name}{bold} [{user.Username}#{user.Discriminator}]");
                    }
                }
            }

            await ReplyAsync(embed: new EmbedBuilder()
                .WithColor(GetColor(Context))
                .WithCurrentTimestamp()
                .WithFooter($"Contestant count: {count}")
                .WithTitle($"Marble {GameName(gameType)}: Contestants")
                .AddField("Contestants", marbleOutput.ToString())
                .Build());
        }

        protected async Task Signup(GameType gameType, string marbleName, int marbleLimit,
            Func<Task> startCommand, Weapon? weapon = null)
        {
            ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
            string marbleListFilePath = $"Data{Path.DirectorySeparatorChar}{fileId}.{GameName(gameType, false)}";
            if (!File.Exists(marbleListFilePath))
            {
                File.Create(marbleListFilePath).Close();
            }

            var binaryFormatter = new BinaryFormatter();
            if (gameType == GameType.Siege)
            {
                if (_gamesService.Sieges.ContainsKey(fileId) && _gamesService.Sieges[fileId].Active)
                {
                    await ReplyAsync($"**{Context.User.Username}**, a battle is currently ongoing!");
                    return;
                }

                using var marbleList = new StreamReader(marbleListFilePath);
                if (marbleList.BaseStream.Length != 0 && ((List<(ulong id, string name)>)binaryFormatter.Deserialize(marbleList.BaseStream)).Any(info => info.id == Context.User.Id))
                {
                    await ReplyAsync($"**{Context.User.Username}**, you've already joined!");
                    return;
                }
            }
            else if (gameType == GameType.War)
            {
                if (_gamesService.Wars.ContainsKey(fileId))
                {
                    await ReplyAsync($"**{Context.User.Username}**, a battle is currently ongoing!");
                    return;
                }

                if (weapon!.WeaponClass == WeaponClass.None || weapon.WeaponClass == WeaponClass.Artillery)
                {
                    await ReplyAsync($"**{Context.User.Username}**, this item cannot be used as a weapon!");
                    return;
                }

                var user = MarbleBotUser.Find(Context);
                if (!user.Items.ContainsKey(weapon.Id) || user.Items[weapon.Id] < 1)
                {
                    await ReplyAsync($"**{Context.User.Username}**, you don't have this item!");
                    return;
                }

                using var marbleList = new StreamReader(marbleListFilePath);
                if (marbleList.BaseStream.Length != 0 && ((List<(ulong id, string name, int itemId)>)binaryFormatter.Deserialize(marbleList.BaseStream)).Any(info => info.id == Context.User.Id))
                {
                    await ReplyAsync($"**{Context.User.Username}**, you've already joined!");
                    return;
                }
            }

            if (string.IsNullOrEmpty(marbleName) || marbleName.StartsWith("<@"))
            {
                marbleName = Context.User.Username;
            }
            else if (marbleName.Length > 100)
            {
                await ReplyAsync($"**{Context.User.Username}**, your entry exceeds the 100 character limit.");
                return;
            }

            string bold = marbleName.Contains('*') || marbleName.Contains('\\') ? "" : "**";
            var builder = new EmbedBuilder()
                .WithColor(GetColor(Context))
                .WithCurrentTimestamp()
                .AddField($"Marble {GameName(gameType)}: Signed up!",
                    $"**{Context.User.Username}** has successfully signed up as {bold}{marbleName}{bold}{(gameType == GameType.War ? $" with the weapon **{weapon!.Name}**" : "")}!");
            using (var mostUsedFile = new StreamWriter($"Data{Path.DirectorySeparatorChar}{GameName(gameType)}MostUsed.txt", true))
            {
                await mostUsedFile.WriteLineAsync(marbleName);
            }

            int marbleNo;
            if (gameType == GameType.War)
            {
                var marbles = new List<(ulong id, string name, int itemId)>();
                using (var marbleFile = new StreamReader(marbleListFilePath))
                {
                    if (marbleFile.BaseStream.Length == 0)
                    {
                        marbleNo = 0;
                    }
                    else
                    {
                        marbles = (List<(ulong id, string name, int itemId)>)binaryFormatter.Deserialize(marbleFile.BaseStream);
                        marbleNo = marbles.Count;
                    }
                }

                marbles.Add((Context.User.Id, marbleName, weapon.Id));

                using (var marbleFile = new StreamWriter(marbleListFilePath))
                {
                    binaryFormatter.Serialize(marbleFile.BaseStream, marbles);
                }
            }
            else
            {
                var marbles = new List<(ulong id, string name)>();
                using (var marbleFile = new StreamReader(marbleListFilePath))
                {
                    if (marbleFile.BaseStream.Length == 0)
                    {
                        marbleNo = 0;
                    }
                    else
                    {
                        marbles = (List<(ulong id, string name)>)binaryFormatter.Deserialize(marbleFile.BaseStream);
                        marbleNo = marbles.Count + 1;
                    }
                }

                marbles.Add((Context.User.Id, marbleName));

                using (var marbleFile = new StreamWriter(marbleListFilePath))
                {
                    binaryFormatter.Serialize(marbleFile.BaseStream, marbles);
                }
            }

            await ReplyAsync(embed: builder.Build());

            if (marbleNo > marbleLimit)
            {
                await ReplyAsync($"The limit of {marbleLimit} contestants has been reached!");
                await startCommand();
            }
        }

        [Command("use", RunMode = RunMode.Async)]
        [Alias("useitem")]
        [Summary("Uses an item.")]
        public async Task UseCommand([Remainder] string searchTerm)
        {
            var item = Item.Find<Item>(searchTerm);

            if (item == null)
            {
                await SendErrorAsync("Could not find the requested item!");
                return;
            }

            var user = MarbleBotUser.Find(Context);

            if (!user.Items.ContainsKey(item.Id) || user.Items[item.Id] == 0)
            {
                await ReplyAsync($"**{Context.User.Username}**, you don't have this item!");
                return;
            }

            void UpdateUser(Item itm, int noOfItems)
            {
                if (user.Items.ContainsKey(itm.Id))
                {
                    user.Items[itm.Id] += noOfItems;
                }
                else
                {
                    user.Items.Add(itm.Id, noOfItems);
                }

                user.NetWorth += item.Price * noOfItems;
                MarbleBotUser.UpdateUser(user);
            }

            if (item is Weapon weapon)
            {
                ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                if (!_gamesService.Sieges.ContainsKey(fileId))
                {
                    await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                    return;
                }

                await _gamesService.Sieges[fileId].WeaponAttack(weapon);
                return;
            }

            switch (item.Id)
            {
                case 1:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.ContainsKey(fileId))
                        {
                            var output = new StringBuilder();
                            var userMarble = _gamesService.Sieges[fileId].Marbles.Find(m => m.Id == Context.User.Id)!;
                            foreach (var marble in _gamesService.Sieges[fileId].Marbles)
                            {
                                marble.Health = marble.MaxHealth;
                                output.AppendLine($"**{marble.Name}** (Health: **{marble.Health}**/{marble.MaxHealth}, DMG: **{marble.DamageDealt}**) [{Context.Client.GetUser(marble.Id).Username}#{Context.Client.GetUser(marble.Id).Discriminator}]");
                            }
                            await ReplyAsync(embed: new EmbedBuilder()
                                .AddField("Marbles", output.ToString())
                                .WithColor(GetColor(Context))
                                .WithCurrentTimestamp()
                                .WithDescription($"**{userMarble.Name}** used **{item.Name}**! Everyone was healed!")
                                .WithTitle(item.Name)
                                .Build());
                            UpdateUser(item, -1);
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 10:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            await siege.ItemAttack(
                                itemId: item.Id,
                                damage: (int)Math.Round(90 + siege.Boss!.MaxHealth * 0.05 * _randomService.Rand.NextDouble() * 0.12 + 0.94));
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 14:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            await siege.ItemAttack(
                                itemId: item.Id,
                                damage: 70 + 10 * (int)siege.Boss!.Difficulty,
                                consumable: true);
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 62:
                case 17:
                    await ReplyAsync("Er... why aren't you using `mb/craft`?");
                    break;
                case 18:
                    if (_gamesService.Scavenges.ContainsKey(Context.User.Id))
                    {
                        UpdateUser(item, -1);
                        if (_gamesService.Scavenges[Context.User.Id].Location == ScavengeLocation.CanaryBeach)
                        {
                            UpdateUser(Item.Find<Item>("019"), 1);
                            await ReplyAsync($"**{Context.User.Username}** dragged a **{item.Name}** across the water, turning it into a **Water Bucket**!");
                        }
                        else if (_gamesService.Scavenges[Context.User.Id].Location == ScavengeLocation.VioletVolcanoes)
                        {
                            UpdateUser(Item.Find<Item>("020"), 1);
                            await ReplyAsync($"**{Context.User.Username}** dragged a **{item.Name}** across the lava, turning it into a **Lava Bucket**!");
                        }
                    }
                    else
                    {
                        await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                    }

                    break;
                case 19:
                case 20:
                    user.Items[item.Id]--;
                    if (user.Items.ContainsKey(19))
                    {
                        user.Items[18]++;
                    }
                    else
                    {
                        user.Items.Add(18, 1);
                    }

                    await ReplyAsync($"**{Context.User.Username}** poured all the water out from a **{item.Name}**, turning it into a **Steel Bucket**!");
                    break;
                case 22:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege) && siege.Boss!.Name == "Help Me the Tree")
                        {
                            var randDish = 22 + _randomService.Rand.Next(0, 13);
                            UpdateUser(item, -1);
                            UpdateUser(Item.Find<Item>(randDish.ToString("000")), 1);
                            await ReplyAsync($"**{Context.User.Username}** used their **{item.Name}**! It somehow picked up a disease and is now a **{Item.Find<Item>(randDish.ToString("000")).Name}**!");
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            var output = new StringBuilder();
                            var userMarble = siege.Marbles.Find(m => m.Id == Context.User.Id)!;
                            foreach (var marble in siege.Marbles)
                            {
                                marble.StatusEffect = StatusEffect.Poison;
                            }

                            await ReplyAsync(embed: new EmbedBuilder()
                                .WithColor(GetColor(Context))
                                .WithCurrentTimestamp()
                                .WithDescription($"**{userMarble.Name}** used **{item.Name}**! Everyone was poisoned!")
                                .WithTitle(item.Name)
                                .Build());
                            UpdateUser(item, -1);
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 38:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            var userMarble = siege.Marbles.Find(m => m.Id == Context.User.Id)!;
                            foreach (var marble in siege.Marbles)
                            {
                                marble.StatusEffect = StatusEffect.Doom;
                            }

                            await ReplyAsync(embed: new EmbedBuilder()
                                .WithColor(GetColor(Context))
                                .WithCurrentTimestamp()
                                .WithDescription($"**{userMarble.Name}** used **{item.Name}**! Everyone is doomed!")
                                .WithTitle(item.Name)
                                .Build());
                            UpdateUser(item, -1);
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 39:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            var userMarble = _gamesService.Sieges[fileId].Marbles.Find(m => m.Id == Context.User.Id)!;
                            userMarble.StatusEffect = StatusEffect.None;
                            await ReplyAsync(embed: new EmbedBuilder()
                                .WithColor(GetColor(Context))
                                .WithCurrentTimestamp()
                                .WithDescription($"**{userMarble.Name}** used **{item.Name}** and is now cured!")
                                .WithTitle(item.Name)
                                .Build());
                            UpdateUser(item, -1);
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                case 57:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (_gamesService.Sieges.TryGetValue(fileId, out Siege? siege))
                        {
                            var userMarble = siege.Marbles.Find(m => m.Id == Context.User.Id)!;
                            userMarble.Evade = 50;
                            userMarble.BootsUsed = true;
                            await ReplyAsync(embed: new EmbedBuilder()
                                .WithColor(GetColor(Context))
                                .WithDescription($"**{userMarble.Name}** used **{item.Name}**, increasing their dodge chance to 50% for the next attack!")
                                .WithTitle($"{item.Name}!")
                                .Build());
                        }
                        break;
                    }
                case 91:
                    {
                        ulong fileId = Context.IsPrivate ? Context.User.Id : Context.Guild.Id;
                        if (!_gamesService.Sieges.ContainsKey(fileId))
                        {
                            _gamesService.Sieges.GetOrAdd(fileId, new Siege(Context, _gamesService, _randomService, Boss.GetBoss("Destroyer"), new List<SiegeMarble>()));
                            await ReplyAsync("*You hear the whirring of machinery...*");
                        }
                        else
                        {
                            await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                        }

                        break;
                    }
                default:
                    await SendErrorAsync($"**{Context.User.Username}**, that item can't be used here!");
                    break;
            }
        }
    }
}
