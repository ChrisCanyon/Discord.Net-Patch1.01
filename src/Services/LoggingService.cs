using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private DiscordBotDBContext _dbcontext;
        private string _logDirectory { get; }
        private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient discord, CommandService commands, DiscordBotDBContext context)
        {
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            _dbcontext = context;
            _discord = discord;
            _commands = commands;
            
            _discord.Log += OnLogAsync;
            _discord.MessageReceived += OnMessageReceivedAsync;
            _commands.Log += OnLogAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;


            var socketCommand = new SocketCommandContext(_discord, msg);

            //Log messages from user
            UserLoggingHelper.LogUserMessage(socketCommand, _dbcontext);

        }

            private Task OnLogAsync(LogMessage msg)
        {

            if (!Directory.Exists(_logDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_logDirectory);
            if (!File.Exists(_logFile))               // Create today's log file if it doesn't exist
                File.Create(_logFile).Dispose();

            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(_logFile, logText + "\n");     // Write the log text to a file

            return Console.Out.WriteLineAsync(logText);       // Write the log text to the console
        }
    }
}
