using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Victoria;
using System.Diagnostics;

namespace bot.Services
{
    public class StartupService
    {
        public static IServiceProvider _provider;
        public readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly LavaNode _lavaNode;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, LavaNode lavaNode)
        {
            _provider = provider;
            _discord = discord;
            _config = config;
            _commands = commands;
            _lavaNode = lavaNode;

            
        }

        public async Task StartAsync()
        {
            string token = _config["discord"];
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("токень хуйня");
                return;
            }
            await _discord.LoginAsync(TokenType.Bot, token);
            await _discord.SetGameAsync("твоё очко");
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
