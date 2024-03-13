using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord.Audio;
using System.Threading.Tasks;
using Victoria;
using bot.Services;
using Victoria.Enums;
using Victoria.Responses;
using Victoria.Resolvers;
using Victoria.Decoder;
using Victoria.EventArgs;
using Victoria.Filters;
using System.Diagnostics;

namespace bot.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly LavaNode _lavaNode;

        public MusicModule(LavaNode lavaNode)
        {
            _lavaNode = lavaNode;
        }
        

        [Command("Join")]
        public async Task JoinAsync()
        {
            if (_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("jopaochko");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("ochkojopa");
                return;
            }

            try
            {
                Console.WriteLine($"Joined {voiceState.VoiceChannel}");
                await _lavaNode.JoinAsync(voiceState.VoiceChannel);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task playAsync([Remainder] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                await ReplyAsync("С даунами не общаюсь");
                return;
            }

            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("Я говна наел");
                return;
            }

            var searchResponse = await _lavaNode.SearchYouTubeAsync(query);
            if (searchResponse.Status == Victoria.Responses.Search.SearchStatus.LoadFailed ||
                searchResponse.Status == Victoria.Responses.Search.SearchStatus.NoMatches)
            {
                await ReplyAsync("Нихуя не нашёл");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == Victoria.Enums.PlayerState.Playing || player.PlayerState == Victoria.Enums.PlayerState.Paused)
            {
                var track = searchResponse.Tracks.First();
                player.Queue.Enqueue(track);
                await ReplyAsync("вроде добавил, но обычно оно не работает");
            }
            else
            {
                var track = searchResponse.Tracks.First();
                await player.PlayAsync(track);
                await ReplyAsync($"А следующая песня звучит специально для пацанов {track.Title}, Длительность {track.Duration}");
            }
        }
    }
}
