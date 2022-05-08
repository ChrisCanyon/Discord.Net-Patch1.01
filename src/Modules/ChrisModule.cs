using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class ChrisModule : ModuleBase<SocketCommandContext>
    {
        private DiscordBotDBContext DBContext;
        private DiscordSocketClient DiscordClient;
        private VoiceConnectivityService VoiceService;

        public ChrisModule(DiscordSocketClient discord, DiscordBotDBContext context, VoiceConnectivityService voice)
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
    }
}
