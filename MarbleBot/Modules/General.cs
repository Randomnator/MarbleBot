﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MarbleBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private static Random rand = new Random();
        const ulong CM = 223616088263491595; // Community Marble
        const ulong THS = 224277738608001024; // The Hat Stoar
        const ulong THSC = 318053169999511554; // The Hat Stoar Crew
        const ulong VFC = 394086559676235776; // Vinh Fan Club
        const ulong ABCD = 412253669392777217; // Blue & Ayumi's Discord Camp
        const ulong MT = 408694288604463114; // Melmon Test
        //const ulong[] THSServers = { THS, THSC, VFC, ABCD, MT };

        struct MBCommand // Used to make stuff in the help command more readable
        {
            public string Name;
            public string Desc;
            public string Usage;
            public string Example;
            public string Warning;
        }

        // Stuff for the jumble command
        public static bool jumbleActive = false;

        //public async Task _jumble2()
        //{
        //    if (jumbleActive && Context.Message.Content.ToLower() == jumbleWord.ToLower()) {
        //        await ReplyAsync("**" + Context.User.Username + "** guessed the word! Well done!");
        //        jumbleActive = false;
        //    }
        //}

        [Command("help")]
        [Summary("Gives the user help.")]
        public async Task _help([Remainder] string command = "")
        {
            if (command == "") {
                EmbedBuilder builder = new EmbedBuilder();

                builder.AddField("MarbleBot Help", "Still a work in progress...")
                    .WithTimestamp(DateTime.UtcNow);

                switch (Context.Guild.Id)
                {
                    case CM:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "8ball (predicts an outcome)\nbest (picks a random user to call the best)\nchoose (chooses between options split with '|')\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nreverse (reverses text)")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays info about a user)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.Teal);
                        break;
                    case THS:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "\n8ball (predicts an outcome)\nbest (picks a random user to call the best)\nbuyhat (buys an Uglee Hat)\nchoose (chooses between options split with '|')\norange (gives a random statement in Orange Language)\norangeify (turns a message you say into Orange Language)\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nstaffcheck (checks the statuses of all staff members)\nvinhglish (shows the meaning and inventor of a Vinhglish word")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays info about a user)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.Orange);
                        break;
                    case THSC:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "\n8ball (predicts an outcome)\nbest (picks a random user to call the best)\nbuyhat (buys an Uglee Hat)\nchoose (chooses between options split with '|')\norange (gives a random statement in Orange Language)\norangeify (turns a message you say into Orange Language)\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nstaffcheck (checks the statuses of all staff members)\nvinhglish (shows the meaning and inventor of a Vinhglish word")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays info about a user)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.Orange);
                        break;
                    case MT:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "\n8ball (predicts an outcome)\nbest (picks a random user to call the best)\nbuyhat (buys an Uglee Hat)\nchoose (chooses between options split with '|')\norange (gives a random statement in Orange Language)\norangeify (turns a message you say into Orange Language)\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nstaffcheck (checks the statuses of all staff members)\nvinhglish (shows the meaning and inventor of a Vinhglish word")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays information about a user; try not to ping if possible)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.DarkGrey);
                        break;
                    case VFC:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "\n8ball (predicts an outcome)\nbest (picks a random user to call the best)\nbuyhat (buys an Uglee Hat)\nchoose (chooses between options split with '|')\norange (gives a random statement in Orange Language)\norangeify (turns a message you say into Orange Language)\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nstaffcheck (checks the statuses of all staff members)\nvinhglish (shows the meaning and inventor of a Vinhglish word")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays info about a user)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.Blue);
                        break;
                    case ABCD:
                        builder.AddField("Command List", "help (should be fairly obvious)")
                            .AddField("Fun Commands", "\n8ball (predicts an outcome)\nbest (picks a random user to call the best)\nbuyhat (buys an Uglee Hat)\nchoose (chooses between options split with '|')\njumble (doesn't work yet)\norange (gives a random statement in Orange Language)\norangeify (turns a message you say into Orange Language)\nrate (rates something out of 10)\nrandom (returns a random positive integer with defined bounds)\nrepeat (repeats a message you say)\nstaffcheck (checks the statuses of all staff members)\nvinhglish (shows the meaning and inventor of a Vinhglish word")
                            .AddField("Utility Commands", "serverinfo (displays information about the server)\nstaffcheck (checks the statuses of all staff members.\nuserinfo (displays information about a user; try not to ping if possible)")
                            .AddField("Role Commands", "give (gives a role)\ntake (takes a role)\nrolelist (lists all roles that can be given/taken)")
                            .AddField("YouTube Commands", "searchchannel (searches for a channel)\nsearchvideo (searches for a video)")
                            .AddField("Games", "\njumble (doesn't work yet)")
                            .WithColor(Color.Gold);
                        break;
                    default:
                        break;
                }
                await ReplyAsync("", false, builder.Build());
            } else {
                MBCommand bCommand = new MBCommand();
                string THSOnly = "This command cannot be used in Community Marble!";
#pragma warning disable CS0162 // Unreachable code detected
                bCommand.Name = command;
                switch(command)
                {
                    // General
                    case "8ball": bCommand.Desc = "Predicts an outcome to an event."; bCommand.Usage = "mb/8ball <condition>"; bCommand.Example = "mb/8ball Will I break?"; break;
                    case "best": bCommand.Desc = "Picks a random user in the server to call the best."; bCommand.Usage = "mb/best"; break;
                    case "buyhat": bCommand.Desc = "Picks a random user in the server to call the best."; bCommand.Usage = "mb/buyhat"; break; bCommand.Warning = THSOnly; break;
                    case "choose": bCommand.Desc = "Chooses between several choices"; bCommand.Usage = "mb/choose <choice1> | <choice2>"; bCommand.Example = "Example: `mb/choose Red | Yellow | Green | Blue"; break;
                    case "jumble": bCommand.Desc = "Does not work."; bCommand.Usage = "mb/jumble"; bCommand.Warning = "This command is completely dysfunctional!"; break;
                    case "orange": bCommand.Desc = "Gives a random statement in Orange Language."; bCommand.Usage = "mb/orange"; bCommand.Warning = THSOnly; break;
                    case "orangeify": bCommand.Desc = "Translates text into Orange Language."; bCommand.Usage = "mb/orangeify <text>"; bCommand.Example = "mb/orangeify Drink Poup Soop!"; bCommand.Warning = THSOnly; break;
                    case "override": bCommand.Desc = "Nothing."; bCommand.Usage = "Don't even think about it."; break;
                    case "random": bCommand.Desc = "Gives a random number between user-defined bounds."; bCommand.Usage = "mb/random <number1> <number2>"; bCommand.Example = "mb/random 1 5"; break;
                    case "rate": bCommand.Desc = "Rates something between 0 and 10."; bCommand.Usage = "mb/rate <text>"; bCommand.Example = "mb/rate Marbles"; break;
                    case "repeat": bCommand.Desc = "Repeats given text."; bCommand.Usage = "mb/repeat <text>"; bCommand.Example = "mb/repeat Hello!"; break;
                    case "reverse": bCommand.Desc = "Reverses text."; bCommand.Usage = "mb/reverse <text>"; bCommand.Example = "mb/reverse Bowl"; break;
                    case "serverinfo": bCommand.Desc = "Displays information about a server."; bCommand.Usage = "mb/serverinfo"; break;
                    case "staffcheck": bCommand.Desc = "Displays a list of all staff members and their statuses."; bCommand.Usage = "mb/staffcheck"; break;
                    case "userinfo": bCommand.Desc = "Displays information about a user."; bCommand.Usage = "mb/userinfo <user>"; bCommand.Example = "mb/userinfo MarbleBot"; bCommand.Warning = "Pinging doesn't work!"; break;
                    case "vinhglish": bCommand.Desc = "Displays information about a Vinhglish word."; bCommand.Usage = "mb/vinglish OR mb/vinhglish <word>"; bCommand.Example = "mb/vinhglish Am Will You"; bCommand.Warning = THSOnly; break;

                    // Roles
                    case "give": bCommand.Desc = "Gives a role if it is on the rolelist"; bCommand.Usage = "mb/give <role>"; bCommand.Example = "mb/give Owner"; break;
                    case "rolelist": bCommand.Desc = "Shows a list of roles that can be given/taken by `mb/give` and `mb/take`."; bCommand.Usage = "mb/rolelist"; break;
                    case "take": bCommand.Desc = "Takes a role if it is on the rolelist"; bCommand.Usage = "mb/take <role>"; bCommand.Example = "mb/take Criminal"; break;

                    // YT
                    case "searchchannel": bCommand.Desc = "Displays a list of channels that match the search criteria."; bCommand.Usage = "mb/searchchannel <channelname>"; bCommand.Example = "mb/searchchannel carykh"; break;
                    case "searchvideo": bCommand.Desc = "Displays a list of videos that match the search critera."; bCommand.Usage = "mb/searchvideo <videoname>"; bCommand.Example = "mb/searchvideo The Amazing Marble Race"; break;
                }

                string message = "";
#pragma warning restore CS0162 // Unreachable code detected

                message += "**__MarbleBot Help: `" + bCommand.Name + "` command__**\n*" + bCommand.Desc + "*\n\nUsage: `" + bCommand.Usage + "`";
                if (!(bCommand.Example == null)) message += "\nExample: `" + bCommand.Example + "`";
                if (!(bCommand.Warning == null)) message += "\n\n:warning: " + bCommand.Warning;
                
                await ReplyAsync(message);
            }
        }

        [Command("cmds")]
        [Summary("Basically just help.")]
        public async Task _cmds([Remainder] string command = "")
        {
            await _help(command);
        }

        //[Command("help2")]
        //[Summary("gives help")]
        //public async Task _help2([Remainder] string command = "")
        //{
        //    EmbedBuilder builder = new EmbedBuilder();

        //    builder.AddField("MarbleBot Help", "Still a work in progress...")
        //        .WithTimestamp(DateTime.UtcNow);

        //    var msg = await ReplyAsync("Uh");
            
        //    var left = new Emoji("⬅️");
        //    var right = new Emoji("➡️");

        //    await msg.AddReactionAsync(left, new RequestOptions());
        //    await msg.AddReactionAsync(right, new RequestOptions());
        //}

        [Command("8ball")]
        [Summary("Predicts an outcome of a user-defined event.")]
        public async Task _8ball([Remainder] string input)
        {
            int choice = rand.Next(0, 13);
            string outcome = "";
            switch (choice)
            {
                case 0:
                    outcome = "no.";
                    break;
                case 1:
                    outcome = "looking negative.";
                    break;
                case 2:
                    outcome = "probably not.";
                    break;
                case 3:
                    outcome = "it is very doubtful.";
                    break;
                case 4:
                    outcome = "my visions are cloudy, try again another time.";
                    break;
                case 5:
                    outcome = "do you *really* want to know?";
                    break;
                case 6:
                    outcome = "too hazy... try again.";
                    break;
                case 7:
                    outcome = "possibly.";
                    break;
                case 8:
                    outcome = "it is highly likely.";
                    break;
                case 9:
                    outcome = "I believe so.";
                    break;
                case 10:
                    outcome = "it is certain.";
                    break;
                case 11:
                    outcome = "and the sign points to... yes!";
                    break;
                case 12:
                    outcome = "and the sign points to... no!";
                    break;
                case 13:
                    outcome = "probably not, but there is still a chance...";
                    break;
            }
            await ReplyAsync(":8ball: |  **" + Context.User.Username + "**, " + outcome);
        }

        [Command("best")]
        [Summary("Picks a random person to call the best")]
        public async Task _best()
        {
            string[] names = new string[Context.Guild.MemberCount];
            SocketGuildUser[] users = Context.Guild.Users.ToArray();
            for (int i = 0; i < Context.Guild.MemberCount - 1; i++)
            {
                names[i] = users[i].Username;
            }
            await ReplyAsync("**" + names[rand.Next(0, Context.Guild.MemberCount - 1)] + "** is the best!");
        }

        [Command("buyhat")]
        [Summary("A user buys an Uglee Hat!")]
        public async Task _buyHat()
        {
            if (Context.Guild.Id == THS || Context.Guild.Id == THSC || Context.Guild.Id == MT || Context.Guild.Id == VFC || Context.Guild.Id == ABCD)
            {
                await ReplyAsync("That'll be " + (rand.Next(0, 10000000)).ToString() + " units of money please. Thank you for buying Uglee Hat #" + (rand.Next(0, 69042)).ToString() + "!");
            }
        }

        [Command("choose")]
        [Summary("Chooses between several choices")]
        public async Task _choose([Remainder] string input)
        {
            string[] choices = input.Split('|');
            int choice = rand.Next(0, choices.Length);
            if (Moderation._checkSwear(input) || Moderation._checkSwear(choices[choice]))
            {
                SocketGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
                if (!Context.IsPrivate) {
                    await ReplyAsync("Profanity detected. " + Doc671.Mention);
                }
            }
            else
            {
                await ReplyAsync("**" + Context.User.Username + "**, I choose **" + choices[choice].Trim() + "**!");
            }
        }

        [Command("jumble")]
        [Summary("Gives the user help. Not done yet.")]
        public async Task _jumble([Remainder] string answer = "")
        {
            string word = "";
            //int timeout = 0;
            if (jumbleActive)
            {
                if (answer == "")
                {
                    await ReplyAsync("A game of jumble is already active!");
                }
                else
                {
                    for(int i = 0; i < answer.Length - 1; i++)
                    {
                        if(answer[i] == ' ')
                        {
                            answer.Replace("", string.Empty);
                        }
                    }
                    if (answer.ToLower() == word.ToLower())
                    {
                        await ReplyAsync("**" + Context.User.Username + "** guessed the word! Well done!");
                        jumbleActive = false;
                    }
                    else
                    {
                        await ReplyAsync("Incorrect.");
                    }
                }
            }
            else
            {
                jumbleActive = true;
                string[] wordList = new string[60];
                int a = 0;
                using (StreamReader stream = new StreamReader("Jumble.csv"))
                {
                    while (!stream.EndOfStream)
                    {
                        string list = stream.ReadLine();
                        wordList[a] = list;
                        Console.WriteLine(list);
                        a += 1;
                    }
                    word = wordList[rand.Next(0, wordList.Length)];
                }

                char[] wordArray = word.ToCharArray();
                Console.WriteLine(wordArray);

                for (a = 0; a < word.Length - 1; a++)
                {
                    int b = rand.Next(0, word.Length - 1);
                    char temp = wordArray[a];
                    wordArray[a] = wordArray[b];
                    wordArray[b] = temp;
                }
                string output = new string(wordArray);
                output = output.ToLower();
                await ReplyAsync("Guess what the word is: **" + output + "**.");
                //do {
                //    var guess = Context.Channel.GetMessagesAsync(1);
                //    guess2 = guess.ToString();
                //    if (guess2.ToLower() == word.ToLower())
                //    {
                //        await ReplyAsync("**" + Context.Message.Author + "** guessed the word! Well done!");
                //        jumbleActive = false;
                //    }
                //    timeout++;
                //    if (timeout > 10000) {
                //        break;
                //    }
                //} while (guess2.ToLower() != word.ToLower());
                //if (timeout > 10000) {
                //    await ReplyAsync("Game over! Nobody could guess the word in time!");
                //    jumbleActive = false;
                //}
                //else if (guess2.ToLower() == word.ToLower())
                //{
                //    await ReplyAsync("**" + Context.User.Username + "** guessed the word! Well done!");
                //    jumbleActive = false;
                //}
                //timeout = 0;
            }
        }

        [Command("orange")]
        [Summary("Gives the user a random statement in Orange Language.")]
        public async Task _orange()
        {
            int choice = rand.Next(1, 6);
            string egnaro = "";
            switch (choice)
            {
                case 1:
                    egnaro = "!olleH";
                    break;
                case 2:
                    egnaro = "!raotS taH ehT owt oG";
                    break;
                case 3:
                    egnaro = "!pooS puoP knirD";
                    break;
                case 4:
                    egnaro = ".depfeQ ,ytiC ogitreV ni evil I";
                    break;
                case 5:
                    egnaro = "!haoW";
                    break;
                case 6:
                    egnaro = "!ainomleM dna dnalkseD ,ytiC ogitreV :depfeQ ni seitic eerht era erehT";
                    break;
            }
            if (Context.Guild.Id == THS || Context.Guild.Id == MT || Context.Guild.Id == VFC || Context.Guild.Id == ABCD) await ReplyAsync(egnaro);
        }

        [Command("orangeify")]
        [Summary("Gives the user a random statement in Orange Language.")]
        public async Task _orangeify([Remainder] string input)
        {
            string orangeified = "";
            int length = input.Length - 1;
            while (length >= 0)
            {
                orangeified += input[length];
                length--;
            }
            if ((Context.Guild.Id == THS || Context.Guild.Id == MT || Context.Guild.Id == VFC || Context.Guild.Id == ABCD)) {
                if (Moderation._checkSwear(input) || Moderation._checkSwear(orangeified)) {
                    IGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
                    await ReplyAsync("Profanity detected. " + Doc671.Mention);
                } else {
                    await ReplyAsync(orangeified);
                }
            }
        }

        [Command("override")]
        [Summary("Desk Is Hacc")]
        public async Task _override(string command)
        {
            if (Context.User.Id == 224267581370925056)
            {
                await ReplyAsync("Overriding command blockages...");
                Thread.Sleep(3000);
                await ReplyAsync("Overriding complete!");
                Thread.Sleep(1000);
                await ReplyAsync("Performing " + command + " command...");
                Thread.Sleep(2000);
                if (command == "buyhat")
                {
                    await _buyHat();
                }
                else if (command == "orange")
                {
                    await _orange();
                }
                else if (command == "orangeify")
                {
                    await ReplyAsync("Unable to perform command.");
                }
                else
                {
                    await ReplyAsync("Unknown command.");
                }
            }
            else
            {
                await ReplyAsync("OVERRIDE FAILURE. INSUFFICIENT PERMISSIONS.");
            }
        }

        [Command("raid")]
        [Summary("Joke command - pretends user is raiding.")]
        public async Task _repeat()
        {
            await ReplyAsync("INITIATING ANTI-RAID PROTOCOL.");
        }

        [Command("random")]
        [Summary("Returns a random number with user-defined bounds.")]
        public async Task _random(int start, int end)
        {
            if (start < 0 || end < 0)
            {
                await ReplyAsync("Only use positive numbers!");
            }
            else if (start > end)
            {
                try
                {
                    int randNumber = rand.Next(end, start);
                    await ReplyAsync(randNumber.ToString());
                }
                catch (FormatException)
                {
                    await ReplyAsync("Number too large/small.");
                    throw;
                }
            }
            else
            {
                try
                {
                    int randNumber = rand.Next(start, end);
                    await ReplyAsync(randNumber.ToString());
                }
                catch (FormatException)
                {
                    await ReplyAsync("Number too large/small.");
                    throw;
                }
            }
        }

        [Command("rate")]
        [Summary("Rates something /10")]
        public async Task _rate([Remainder] string input)
        {
            int rating = 0;
            string message = "";
            string emoji = "";
            switch (input.ToLower())
            {
                // These inputs have custom ratings and messages:
                case "256 mg":
                    rating = -2;
                    message = "I Am In Confusial Why";
                    break;
                case "ddsc":
                    rating = 0;
                    break;
                case "desk":
                    rating = 11;
                    message = "what did you expect?";
                    break;
                case "desk176":
                    rating = 11;
                    message = "what did you expect?";
                    break;
                case "desks":
                    rating = 11;
                    message = "what did you expect?";
                    break;
                case "doc671":
                    rating = -1;
                    message = "terrible at everything";
                    break;
                case "erango":
                    rating = 0;
                    message = "stoP noW";
                    break;
                case "flask":
                    rating = -1;
                    message = "don't you dare";
                    break;
                case ":regional_indicator_f::flag_la::flag_sk:":
                    rating = -1;
                    message = "don't you dare";
                    break;
                case ":regional_indicator_f::regional_indicator_l::regional_indicator_a::regional_indicator_s::regional_indicator_k:":
                    rating = -1;
                    message = "don't you dare";
                    break;
                case ":regional_indicator_f: :regional_indicator_l: :regional_indicator_a: :regional_indicator_s: :regional_indicator_k:":
                    rating = -1;
                    message = "don't you dare";
                    break;
                case "magenta curse":
                    rating = 0;
                    message = "stuck in the old ways, I see, tut-tut...";
                    break;
                case "magenta virus":
                    rating = 0;
                    message = "doesn't exist anymore; stop";
                    break;
                case "orange":
                    rating = 10;
                    message = "!egnarO";
                    break;
                case "poup soop":
                    rating = 10;
                    message = "considering who made this bot...";
                    break;
                case "table":
                    rating = -999;
                    message = "do not mention that unholy creature near me";
                    input = "dGFibGU=";
                    break;
                case "tables":
                    rating = -999;
                    message = "do not mention those unholy creatures near me";
                    input = "dGFibGVz";
                    break;
                case "the hat stoar":
                    rating = 10;
                    message = "!raotS taH ehT owt oG";
                    break;
                case "vinh":
                    rating = 10;
                    message = "Henlo Cooooooooool Vinh";
                    break;
                // If the input is none of the above, randomise the rating:
                default:
                    rating = rand.Next(0, 10);
                    break;
            }
            switch (input.ToLower())
            {
                // These have custom messages but no preset ratings:
                case "blueice57":
                    message = "icccce";
                    break;
                case "flam":
                    message = "Am Flam Flam Flam Flam Flam Flam";
                    break;
                case "flame":
                    message = "Am Flam Flam Flam Flam Flam Flam";
                    break;
                case "flamevapour":
                    message = "Am Flam Flam Flam Flam Flam Flam";
                    break;
                case "flurp":
                    message = "FLURP I TO SIGN UP AND NOT BE";
                    break;
                case "george012":
                    message = "henlo jorj";
                    break;
                case "icce":
                    message = "icccce";
                    break;
                case "jorj":
                    message = "henlo jorj";
                    break;
                case "ken":
                    message = "#kenismelmon";
                    break;
                case "kenlimepie":
                    message = "#kenismelmon";
                    break;
                case "keylimepie":
                    message = "#kenismelmon";
                    break;
                case "matheus":
                    message = "marbles will realize in +inf";
                    break;
                case "matheus fazzion":
                    message = "marbles will realize in +inf";
                    break;
                case "meadow":
                    message = "somebody toucha mei doe";
                    break;
                case "meidoe":
                    message = "somebody toucha mei doe";
                    break;
                case "melmon":
                    message = "Wnhy Arey Yoou A Melmon";
                    break;
                case "shotgun":
                    message = "Vinh Shotgun All";
                    break;
            }
            if (input.ToLower() == "dann" || input.ToLower() == "danny playz")
            {
                int choice = rand.Next(0, 2);
                if (choice == 1)
                {
                    message = "you guys, are a rat kids";
                }
                else
                {
                    message = "I don’t know who you are I don’t know what you want but if I don’t get my t-shirt tomorrow i will find you and I will rob you.";
                }
                rating = rand.Next(9, 10);
            }
            bool[] flask = { false, false, false, false, false };
            for (int i = 0; i < input.Length - 1; i++)
            {
                switch (input[i].ToString().ToLower())
                {
                    case "f": flask[0] = true; break;
                    case "l": flask[1] = true; break;
                    case "a": flask[2] = true; break;
                    case "s": flask[3] = true; break;
                    case "k": flask[4] = true; break;
                }
            }
            if (flask[0] == true && flask[1] == true && flask[2] == true && flask[3] == true && flask[4] == true)
            {
                rating = -1;
                message = "don't you dare";
            }
            switch (rating)
            {
                // Emoji time!
                case -999:
                    emoji = ":gun: :dagger: :bomb:";
                    break;
                case -1:
                    emoji = ":gun:";
                    break;
                case 0:
                    emoji = ":no_entry_sign:";
                    break;
                case 1:
                    emoji = ":nauseated_face:";
                    break;
                case 2:
                    emoji = ":rage:";
                    break;
                case 3:
                    emoji = ":angry:";
                    break;
                case 4:
                    emoji = ":slight_frown:";
                    break;
                case 5:
                    emoji = ":neutral_face:";
                    break;
                case 6:
                    emoji = ":slight_smile:";
                    break;
                case 7:
                    emoji = ":grinning:";
                    break;
                case 8:
                    emoji = ":thumbsup:";
                    break;
                case 9:
                    emoji = ":white_check_mark:";
                    break;
                case 10:
                    emoji = ":rofl:";
                    break;
                case 11:
                    emoji = ":heart:";
                    break;
                default:
                    emoji = ":thinking:";
                    break;
            }
            if (message == "")
            {
                switch (rating)
                {
                    // If there isn't already a custom message, pick one depending on rating:
                    case 0:
                        message = "Excuse me, kind sir/madam, please cease your current course of action immediately.";
                        break;
                    case 1:
                        message = "Immediate desistance required.";
                        break;
                    case 2:
                        message = "I don't like it...";
                        break;
                    case 3:
                        message = "angery";
                        break;
                    case 4:
                        message = "ehhh...";
                        break;
                    case 5:
                        message = "not bad... but not good either";
                        break;
                    case 6:
                        message = "slightly above average... I guess...";
                        break;
                    case 7:
                        message = "pretty cool, don't you think?";
                        break;
                    case 8:
                        message = "yes";
                        break;
                    case 9:
                        message = "approaching perfection";
                        break;
                    case 10:
                        message = "PERFECT!!";
                        break;
                    default:
                        message = "Uhhhhhhhh\nNot";
                        break;
                }
            }
            if (rating == -2)
            {
                await ReplyAsync("**" + Context.User.Username + "**, I rATE " + input + " UNd3FINED10. " + emoji + "\n(" + message + ")");
            }
            else
            {
                if (Moderation._checkSwear(input))
                {
                    IGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
                    await ReplyAsync("Profanity detected. " + Doc671.Mention);
                }
                else
                {
                    await ReplyAsync("**" + Context.User.Username + "**, I rate " + input + " **" + rating + "**/10. " + emoji + "\n(" + message + ")");
                }
            }
        }

        [Command("repeat")]
        [Summary("Repeats the message they say.")]
        public async Task _repeat([Remainder] string repeat)
        {
            if (repeat == "Am Melmon" && (Context.Guild.Id == THS || Context.Guild.Id == MT))
            {
                await ReplyAsync("No U");
            }
            else if (Moderation._checkSwear(repeat))
            {
                IGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
                await ReplyAsync("Profanity detected. " + Doc671.Mention);
            }
            else
            {
                await ReplyAsync(repeat);
            }
        }

        [Command("reverse")]
        [Summary("Gives the user a random statement in Orange Language.")]
        public async Task _reverse([Remainder] string input)
        {
            // Another version of orangeify, but for CM (can secretly be used elsewhere)
            string reverse = "";
            int length = input.Length - 1;
            while (length >= 0) {
                reverse += input[length];
                length--;
            }
            if(Moderation._checkSwear(input) || Moderation._checkSwear(reverse)) {
                IGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
                await ReplyAsync("Profanity detected. " + Doc671.Mention);
            } else {
                await ReplyAsync(reverse);
            }
        }

        [Command("serverinfo")]
        [Summary("Returns some stats")]
        public async Task _serverinfo()
        {
            EmbedBuilder builder = new EmbedBuilder();
            Color coloure = Color.LightGrey;
            int botUsers = 0;
            int onlineUsers = 0;
            SocketGuildUser[] users = Context.Guild.Users.ToArray();
            switch (Context.Guild.Id)
            {
                case CM: coloure = Color.Teal; break;
                case THS: coloure = Color.Orange; break;
                case MT: coloure = Color.DarkGrey; break;
                case VFC: coloure = Color.Blue; break;
                case ABCD: coloure = Color.Gold; break;
            }
            for (int i = 0; i < Context.Guild.Users.Count - 1; i++)
            {
                if (users[i].IsBot) botUsers++;
                if (users[i].Status.ToString().ToLower() == "online") onlineUsers++;
            }
            builder.WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle(Context.Guild.Name)
                .AddInlineField("Owner", Context.Guild.GetUser(Context.Guild.OwnerId).Username + "#" + Context.Guild.GetUser(Context.Guild.OwnerId).Discriminator)
                .AddInlineField("Voice Region", Context.Guild.VoiceRegionId)
                .AddInlineField("Text Channels", Context.Guild.TextChannels.Count)
                .AddInlineField("Voice Channels", Context.Guild.VoiceChannels.Count)
                .AddInlineField("Members", Context.Guild.Users.Count)
                .AddInlineField("Bots", botUsers)
                .AddInlineField("Online", onlineUsers)
                .AddInlineField("Roles", Context.Guild.Roles.Count)
                .WithColor(coloure)
                .WithTimestamp(DateTime.UtcNow)
                .WithFooter(Context.Guild.Id.ToString());
            await ReplyAsync("", false, builder.Build());
        }

        [Command("stats")]
        [Summary("Returns some stats")]
        public async Task _stats()
        {
            string mensage = "";
            SocketGuildChannel[] channels = Context.Guild.TextChannels.ToArray();
            Array.Sort(channels, (x, y) => String.Compare(x.Name, y.Name));
            for (int i = 0; i < Context.Guild.Channels.Count - 1; i++) mensage += channels[i] + " ";
            await ReplyAsync(mensage);
        }

        [Command("staffcheck")]
        [Summary("Checks which staff are online/idle/DND/offline.")]
        public async Task _staffCheck()
        {
            IGuildUser Doc671 = Context.Guild.GetUser(224267581370925056);
            if (Context.Guild.Id == CM)
            {
                IGuildUser Erikfassett = Context.Guild.GetUser(161258738429329408);
                IGuildUser JohnDubuc = Context.Guild.GetUser(161247044713840642);
                IGuildUser TAR = Context.Guild.GetUser(186652039126712320);
                IGuildUser BradyForrest = Context.Guild.GetUser(211110948566597633);
                IGuildUser Algorox = Context.Guild.GetUser(323680030724980736);
                IGuildUser FlameVapour = Context.Guild.GetUser(193247613095641090);
                IGuildUser Small = Context.Guild.GetUser(222125122020966400);
                IGuildUser[] users = { Doc671, Erikfassett, JohnDubuc, TAR, Algorox, BradyForrest, FlameVapour, Small };
                string[] nicks = { users[0].Nickname, users[1].Nickname, users[2].Nickname, users[3].Nickname, users[4].Nickname, users[5].Nickname, users[6].Nickname, users[7].Nickname };
                string[] statuses = { users[0].Status.ToString(), users[1].Status.ToString(), users[2].Status.ToString(), users[3].Status.ToString(), users[4].Status.ToString(), users[5].Status.ToString(), users[6].Status.ToString(), users[7].Status.ToString() };
                for (int i = 0; i < users.Length; i++)
                {
                    if (nicks[i] == "" || nicks[i] == null || nicks[i] == "  ")
                    {
                        nicks[i] = users[i].Username;
                    }
                    if (statuses[i] == "DoNotDisturb")
                    {
                        statuses[i] = "Do Not Disturb";
                    }
                }
                await ReplyAsync("**__Admins:__** \n" + nicks[0] + " (" + users[0].Username + "#" + users[0].Discriminator + "): **" + statuses[0] + "**\n" + nicks[1] + " (" + users[1].Username + "#" + users[1].Discriminator + "): **" + statuses[1] + "**\n" + nicks[2] + " (" + users[2].Username + "#" + users[2].Discriminator + "): **" + statuses[2] + "**\n" + nicks[3] + " (" + users[3].Username + "#" + users[3].Discriminator + "): **" + statuses[3] + "**\n\n**__Mods:__**\n" + nicks[4] + " (" + users[4].Username + "#" + users[4].Discriminator + "): **" + statuses[4] + "**\n" + nicks[5] + " (" + users[5].Username + "#" + users[5].Discriminator + "): **" + statuses[5] + "**\n" + nicks[6] + " (" + users[6].Username + "#" + users[6].Discriminator + "): **" + statuses[6] + "**\n" + nicks[7] + " (" + users[7].Username + "#" + users[7].Discriminator + "): **" + statuses[7] + "**");
            }
            else if (Context.Guild.Id == THS)
            {
                IGuildUser FlameVapour = Context.Guild.GetUser(193247613095641090);
                IGuildUser BradyForrest = Context.Guild.GetUser(211110948566597633);
                IGuildUser DannyPlayz = Context.Guild.GetUser(329532528031563777);
                IGuildUser George012 = Context.Guild.GetUser(232618363975630849);
                IGuildUser Kenlimepie = Context.Guild.GetUser(195529549855850496);
                IGuildUser[] users = { Doc671, FlameVapour, BradyForrest, DannyPlayz, George012, Kenlimepie };
                string[] nicks = { users[0].Nickname, users[1].Nickname, users[2].Nickname, users[3].Nickname, users[4].Nickname, users[5].Nickname };
                string[] statuses = { users[0].Status.ToString(), users[1].Status.ToString(), users[2].Status.ToString(), users[3].Status.ToString(), users[4].Status.ToString(), users[5].Status.ToString() };
                for (int i = 0; i < users.Length; i++)
                {
                    if (nicks[i] == "" || nicks[i] == null || nicks[i] == "  ")
                    {
                        nicks[i] = users[i].Username;
                    }
                    if (statuses[i] == "DoNotDisturb")
                    {
                        statuses[i] = "Do Not Disturb";
                    }
                    Console.WriteLine(nicks[i]);
                }
                await ReplyAsync("**__Overlords:__** \n" + nicks[0] + " (" + users[0].Username + "#" + users[0].Discriminator + "): **" + statuses[0] + "**\n" + nicks[1] + " (" + users[1].Username + "#" + users[1].Discriminator + "): **" + statuses[1] + "**\n\n**__Hat Stoar Managers:__**\n" + nicks[2] + " (" + users[2].Username + "#" + users[2].Discriminator + "): **" + statuses[2] + "**\n\n**__Hat Stoar Employees:__**\n" + nicks[3] + " (" + users[3].Username + "#" + users[3].Discriminator + "): **" + statuses[3] + "**\n" + nicks[4] + " (" + users[4].Username + "#" + users[4].Discriminator + "): **" + statuses[4] + "**\n" + nicks[5] + " (" + users[5].Username + "#" + users[5].Discriminator + "): **" + statuses[5] + "**");
            }
            else if (Context.Guild.Id == MT)
            {
                IGuildUser George012 = Context.Guild.GetUser(232618363975630849);
                IGuildUser[] users = { Doc671, George012 };
                string[] nicks = { users[0].Nickname, users[1].Nickname, };
                string[] statuses = { users[0].Status.ToString(), users[1].Status.ToString() };
                for (int i = 0; i < users.Length; i++)
                {
                    if (nicks[i] == "" || nicks[i] == null || nicks[i] == "  ")
                    {
                        nicks[i] = users[i].Username;
                    }
                    if (statuses[i] == "DoNotDisturb")
                    {
                        statuses[i] = "Do Not Disturb";
                    }
                }
                await ReplyAsync(nicks[0] + " (" + users[0].Username + "#" + users[0].Discriminator + "): **" + statuses[0] + "**\n" + nicks[1] + " (" + users[1].Username + "#" + users[1].Discriminator + "): **" + statuses[1] + "**");
            }
            else if (Context.Guild.Id == VFC)
            {
                IGuildUser Vinh = Context.Guild.GetUser(311360247740760064);
                IGuildUser George012 = Context.Guild.GetUser(232618363975630849);
                IGuildUser Kenlimepie = Context.Guild.GetUser(195529549855850496);
                IGuildUser Matheus = Context.Guild.GetUser(403595947537334274);
                IGuildUser Nihonium = Context.Guild.GetUser(233912334517534720);
                IGuildUser Petrified = Context.Guild.GetUser(373322546084577280);
                IGuildUser Ayumi = Context.Guild.GetUser(189713815414374404);
                IGuildUser Quackitye = Context.Guild.GetUser(371575413115322379);
                IGuildUser BTMR = Context.Guild.GetUser(378875092538621963);
                IGuildUser Holly = Context.Guild.GetUser(210397169075748864);
                IGuildUser ZeeTaa = Context.Guild.GetUser(373086863369699328);
                IGuildUser[] users = { Vinh, George012, Kenlimepie, Matheus, Nihonium, Petrified, Ayumi, Quackitye, BTMR, Doc671, Holly, ZeeTaa };
                string[] nicks = { users[0].Nickname, users[1].Nickname, users[2].Nickname, users[3].Nickname, users[4].Nickname, users[5].Nickname, users[6].Nickname, users[7].Nickname, users[8].Nickname, users[9].Nickname, users[10].Nickname, users[11].Nickname };
                string[] statuses = { users[0].Status.ToString(), users[1].Status.ToString(), users[2].Status.ToString(), users[3].Status.ToString(), users[4].Status.ToString(), users[5].Status.ToString(), users[6].Status.ToString(), users[7].Status.ToString(), users[8].Status.ToString(), users[9].Status.ToString(), users[10].Status.ToString(), users[11].Status.ToString() };
                for (int i = 0; i < users.Length; i++)
                {
                    if (nicks[i] == "" || nicks[i] == null || nicks[i] == "  ")
                    {
                        nicks[i] = users[i].Username;
                    }
                    if (statuses[i] == "DoNotDisturb")
                    {
                        statuses[i] = "Do Not Disturb";
                    }
                }
                await ReplyAsync("**__Owner:__** \n" + nicks[0] + " (" + users[0].Username + "#" + users[0].Discriminator + "): **" + statuses[0] + "**\n\n**__Co-owners:__** \n" + nicks[1] + " (" + users[1].Username + "#" + users[1].Discriminator + "): **" + statuses[1] + "**\n" + nicks[2] + " (" + users[2].Username + "#" + users[2].Discriminator + "): **" + statuses[2] + "**\n" + nicks[3] + " (" + users[3].Username + "#" + users[3].Discriminator + "): **" + statuses[3] + "**\n" + nicks[4] + " (" + users[4].Username + "#" + users[4].Discriminator + "): **" + statuses[4] + "**\n" + nicks[5] + " (" + users[5].Username + "#" + users[5].Discriminator + "): **" + statuses[5] + "**\n\n**__Admins:__** \n" + nicks[6] + " (" + users[6].Username + "#" + users[6].Discriminator + "): **" + statuses[6] + "**\n" + nicks[7] + " (" + users[7].Username + "#" + users[7].Discriminator + "): **" + statuses[7] + "**\n" + nicks[8] + " (" + users[8].Username + "#" + users[8].Discriminator + "): **" + statuses[8] + "**\n" + nicks[9] + " (" + users[9].Username + "#" + users[9].Discriminator + "): **" + statuses[9] + "**\n" + nicks[10] + " (" + users[10].Username + "#" + users[10].Discriminator + "): **" + statuses[10] + "**\n" + nicks[11] + " (" + users[11].Username + "#" + users[11].Discriminator + "): **" + statuses[11] + "**");
            }
            else if (Context.Guild.Id == ABCD)
            {
                IGuildUser BTMR = Context.Guild.GetUser(378875092538621963);
                IGuildUser Ayumi = Context.Guild.GetUser(189713815414374404);
                IGuildUser[] users = { BTMR, Ayumi };
                string[] nicks = { users[0].Nickname, users[1].Nickname, };
                string[] statuses = { users[0].Status.ToString(), users[1].Status.ToString() };
                for (int i = 0; i < users.Length; i++)
                {
                    if (nicks[i] == "" || nicks[i] == null || nicks[i] == "  ")
                    {
                        nicks[i] = users[i].Username;
                    }
                    if (statuses[i] == "DoNotDisturb")
                    {
                        statuses[i] = "Do Not Disturb";
                    }
                }
                await ReplyAsync("**__Hosts:__**\n" + nicks[0] + " (" + users[0].Username + "#" + users[0].Discriminator + "): **" + statuses[0] + "**\n" + nicks[1] + " (" + users[1].Username + "#" + users[1].Discriminator + "): **" + statuses[1] + "**");
            }
        }

        [Command("userinfo")]
        [Summary("Returns info of a user")]
        public async Task _userinfo([Remainder] string username = "#")
        {
            EmbedBuilder builder = new EmbedBuilder();
            Color coloure = Color.LightGrey;
            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            switch (Context.Guild.Id)
            {
                case CM: coloure = Color.Teal; break;
                case THS: coloure = Color.Orange; break;
                case MT: coloure = Color.DarkGrey; break;
                case VFC: coloure = Color.Blue; break;
                case ABCD: coloure = Color.Gold; break;
            }
            char[] userCharArray = username.ToCharArray();
            int likeness = 0;
            bool chosen = false;
            if (username != "#")
            {
                SocketGuildUser[] users = Context.Guild.Users.ToArray();
                for(int i = 0; i < users.Length - 1; i++) // Iterates through all users
                {
                    for(int j = 0; j < users[i].Username.Length - 1; j++) // Iterates through all characters in the user's name
                    {
                        char[] userNameArray = users[i].Username.ToCharArray();
                        for(int k = 0; k < userCharArray.Length - 1; k++) // Iterates through all characters in the given name
                        {
                            if (userCharArray[k] == userNameArray[k]) likeness++; 
                        }
                    }
                    if (likeness > (userCharArray.Length / 2)) user = users[i]; chosen = true;
                    //if (users[i].Username == username) user = users[i];
                    //else if (users[i].Nickname == username) user = users[i];
                    //else username.Replace(" ", "");
                    //    if (users[i].Username == username) user = users[i];
                    //    else if (users[i].Nickname == username) user = users[i];
                }
                if (!chosen) await ReplyAsync("Could not find requested user.");
            }

            string status = "";
            switch (Context.User.Status.ToString()) {
                case "online": status = "Online"; break;
                case "idle": status = "Idle"; break;
                case "donotdisturb": status = "Do Not Disturb"; break;
                case "invisible": status = "Offline"; break;
                case "offline": status = "Offline"; break;
            }

            string nickname = "";
            if (user.Nickname == "" || user.Nickname == null) nickname = "None";
            else nickname = user.Nickname;

            builder.WithThumbnailUrl(Context.User.GetAvatarUrl())
                .WithTitle(Context.User.Username + "#" + Context.User.Discriminator)
                .AddField("Status", status)
                .AddInlineField("Nickname", nickname)
                .AddField("Registered", user.CreatedAt)
                .AddInlineField("Joined", user.JoinedAt)
                .AddField("Roles", user.Roles)
                .WithColor(coloure)
                .WithTimestamp(DateTime.UtcNow)
                .WithFooter(Context.Guild.Id.ToString());

            await ReplyAsync("", false, builder.Build());
        }

        [Command("vinhglish")]
        [Summary("Returns a Vinhglish word, its inventor and meaning")]
        public async Task _vinhglish([Remainder] string word = "")
        {
            int randNo = 0;
            bool wordSet = false;
            string[] wordList = new string[100];
            string[] invList = new string[100];
            string[] descList = new string[100];
            int a = 0;
            if (word == "") {
                using (StreamReader stream = new StreamReader("Vinhglish.csv")) {
                    while (!stream.EndOfStream) {
                        string list = stream.ReadLine();
                        string[] vocab = list.Split(',');
                        wordList[a] = vocab[0];
                        invList[a] = vocab[1];
                        descList[a] = vocab[2];
                        a++;
                    }
                }
                randNo = rand.Next(1, a);
            } else {
                using (StreamReader stream = new StreamReader("Vinhglish.csv")) {
                    while (!stream.EndOfStream)
                    {
                        string list = stream.ReadLine();
                        string[] vocab = list.Split(',');
                        wordList[a] = vocab[0];
                        invList[a] = vocab[1];
                        descList[a] = vocab[2];
                        if (wordList[a].ToLower() == word.ToLower())
                        {
                            randNo = a;
                            stream.Close();
                            wordSet = true;
                            break;
                        }
                        //JGeoroegeos
                        a++;
                    }
                    if (!wordSet) randNo = rand.Next(1, a);
                }
            }
            if (Context.Guild.Id == THS || Context.Guild.Id == MT || Context.Guild.Id == VFC || Context.Guild.Id == ABCD) await ReplyAsync("**__" + wordList[randNo] + "__**\nInventor: " + invList[randNo] + "\nDescription: " + descList[randNo]);
        }

        [Command("deskismelmon")]
        [Summary("deskismelmon")]
        public async Task _deskismelmon([Remainder] string msg)
        {
            if (Context.User.Id == 224267581370925056) { 
                Console.WriteLine("Time For MElmonry >:)");
                SocketGuild TheHatStoar = Program._client.GetGuild(THS);
                ISocketMessageChannel THSGeneral = TheHatStoar.GetTextChannel(THS);
                await THSGeneral.SendMessageAsync(msg);
            }
        }
    }
}
