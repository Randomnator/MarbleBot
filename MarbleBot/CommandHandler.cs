﻿using Discord.Commands;
using Discord.WebSocket;
using MarbleBot.Core;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarbleBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Global.CommandService = _service;
            _client.MessageReceived += HandleCommandAsync;
        }

        public string Name { get; }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;

            var server = new MBServer(0);

            if (!context.IsPrivate) 
            {
                if (Global.Servers.Value.Any(sr => sr.Id == context.Guild.Id))
                    server =  MarbleBotModule.GetServer(context);
                else {
                    server = new MBServer(context.Guild.Id);
                    Global.Servers.Value.Add(server);
                }
            }

            if (msg.HasStringPrefix("mb/", ref argPos) && msg.Author.IsBot == false && (context.IsPrivate || 
                server.UsableChannels.Count == 0 || server.UsableChannels.Contains(context.Channel.Id))) {
                var result = await _service.ExecuteAsync(context, argPos, null);
                
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await Program.Log($"{result.Error.Value}: {result.ErrorReason}");

                switch (result.Error) 
                {
                    case CommandError.BadArgCount: await context.Channel.SendMessageAsync("Wrong number of arguments. Use `mb/help <command name>` to see how to use the command."); break;
                    case CommandError.UnmetPrecondition: await context.Channel.SendMessageAsync("Insufficient permissions."); break;
                }

            } else if (!context.IsPrivate && server.AutoresponseChannel == context.Channel.Id
                && DateTime.UtcNow.Subtract(Global.ARLastUse).TotalSeconds > 2) {
                foreach (var response in Global.Autoresponses) {
                    if (string.Compare(context.Message.Content, response.Key, true) == 0) {
                        Global.ARLastUse = DateTime.UtcNow;
                        await context.Channel.SendMessageAsync(response.Value); break;
                    }
                }
            }
            if (context.IsPrivate) await Program.Log($"{context.User}: {context.Message}");
        }
    }
}