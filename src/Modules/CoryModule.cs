using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Models;
using System.Collections.Generic;

namespace DiscordBot.Modules
{
    [Name("Yaigh")]
    public class CoryModule : ModuleBase<SocketCommandContext>
    {
        private DiscordBotDBContext DBContext;
        private DiscordSocketClient DiscordClient;
        private VoiceConnectivityService VoiceService;

        public CoryModule(DiscordSocketClient discord, DiscordBotDBContext context, VoiceConnectivityService voice)
        {
            DBContext = context;
            DiscordClient = discord;
            VoiceService = voice;
        }

        
    }
}
