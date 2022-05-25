using System;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.API;
using Discord.Audio;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.Rest;
using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using bot.Commands;
using System.Deployment.Application;
using Victoria;
using Victoria.Enums;

namespace bot
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}