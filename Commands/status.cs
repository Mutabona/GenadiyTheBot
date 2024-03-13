using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;
using Discord;
using bot.Services;
using DiscordRPC;
using DiscordRPC.Message;

namespace bot.Commands
{
    public class status : ModuleBase<SocketCommandContext>
    {
        [Command("status")]
        [RequireRole("Верховный гей")]
        public async Task statusAsync(params string[] msg)
        {
            var user = Context.User;
            string status = "";
            foreach (string st in msg)
            {
                status += " " + st;
            }
            Console.WriteLine($"Status changed to {status}");
            await Context.Client.SetGameAsync(Convert.ToString(status));
        }
    }
}
