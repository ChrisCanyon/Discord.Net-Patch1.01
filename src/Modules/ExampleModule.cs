using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Models;
using System.Collections.Generic;

namespace DiscordBot.Modules
{
    [Name("Example")]
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        private DiscordBotDBContext DBContext;
        private DiscordSocketClient DiscordClient;
        private VoiceConnectivityService VoiceService;

        public ExampleModule(DiscordSocketClient discord, DiscordBotDBContext context, VoiceConnectivityService voice)
        {
            DBContext = context;
            DiscordClient = discord;
            VoiceService = voice;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            VoiceService.HandleAudioConnection(channel);
        }

        [Command("say"), Alias("s")]
        [Summary("Make the bot say something")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task Say([Remainder]string text)
        {
            return ReplyAsync(text);
        }

        [Command("taco"), Alias("t")]
        [Summary("Make the bot say something")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task Test([Remainder] string text)
        {
            var GetDataTask = DBContext.DiscordUsers.ToArrayAsync();

            List<DiscordUser> Users = new List<DiscordUser>(await GetDataTask);

            SocketUser user = Context.User;

            await ReplyAsync(text);
        }

        [Group("set"), Name("Example")]
        [RequireContext(ContextType.Guild)]
        public class Set : ModuleBase
        {
            [Command("nick"), Priority(1)]
            [Summary("Change your nickname to the specified text")]
            [RequireUserPermission(GuildPermission.ChangeNickname)]
            public Task Nick([Remainder]string name)
                => Nick(Context.User as SocketGuildUser, name);

            [Command("nick"), Priority(0)]
            [Summary("Change another user's nickname to the specified text")]
            [RequireUserPermission(GuildPermission.ManageNicknames)]
            public async Task Nick(SocketGuildUser user, [Remainder]string name)
            {
                await user.ModifyAsync(x => x.Nickname = name);
                await ReplyAsync($"{user.Mention} I changed your name to **{name}**");
            }
        }
    }
}
