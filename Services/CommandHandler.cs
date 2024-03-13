using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Victoria;
using System.Diagnostics;

namespace bot.Services
{
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static CommandService _commands;
        private static IConfigurationRoot _config;
        private readonly LavaNode _lavaNode;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider, LavaNode lavaNode)
        {
            _provider = provider;
            _discord = discord;
            _config = config;
            _commands = commands;
            _lavaNode = lavaNode;

            _discord.Ready += OnReadyAsync;
            _discord.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;

            if (msg.Author.IsBot) return;
            var context = new SocketCommandContext(_discord, msg);

            int pos = 0;
            if(msg.HasStringPrefix(_config["prefix"], ref pos))
            {
                var result = await _commands.ExecuteAsync(context, pos, _provider);
                if (!result.IsSuccess)
                {
                    var reason = result.Error;
                    Console.WriteLine(reason);
                }
            }
        }

        private async Task OnReadyAsync()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            if (!_lavaNode.IsConnected)
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = "Lavalink.jar";
                process.StartInfo.WorkingDirectory = @"C:\Users\kodo2\source\repos\bot\Modules\LavaLink";
                process.Start();
                Console.WriteLine("Lavalink started");
                await _lavaNode.ConnectAsync();
                Console.WriteLine("LavaNode connected");
            }
        }
    }
}
