using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class VoiceConnectivityService
    {
        private readonly DiscordSocketClient DiscordClient;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfiguration, and IServiceProvider are injected automatically from the IServiceProvider
        public VoiceConnectivityService(
            DiscordSocketClient discord,
            CommandService commands,
            IConfiguration config,
            IServiceProvider provider)
        {
            DiscordClient = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            Connections = new Dictionary<ulong, AudioConnection>();
        }

        Dictionary<ulong, AudioConnection> Connections;

        public class AudioConnection
        {
            public AudioConnection(IAudioClient client, IVoiceChannel channel)
            {
                this.client = client;
                this.channel = channel;
            }
            public IAudioClient client { get; set; }
            public IVoiceChannel channel { get; set; }
            public AudioOutStream stream { get; set; }
        }

        private AudioConnection AddConnection(IAudioClient client, IVoiceChannel channel)
        {
            client.Disconnected += OnDisconnect;
            var connection = new AudioConnection(client, channel);
            Connections[channel.Id] = connection;
            return connection;
        }

        public async Task OnDisconnect(Exception e)
        {
            var staleConnections = Connections.Where(x => x.Value.client.ConnectionState == ConnectionState.Disconnected || x.Value.client.ConnectionState == ConnectionState.Disconnecting).ToList();
            staleConnections.ForEach(x => Connections.Remove(x.Key));
        }

        private async Task<AudioConnection> SyncAudioConnectionsAsync(IVoiceChannel channel)
        {
            AudioConnection connection;
            //check if bot is already in channel
            if (await channel.GetUserAsync(DiscordClient.CurrentUser.Id) != null)
            {
                //Check for client in connections list
                Connections.TryGetValue(channel.Id, out connection);

                //If it doesnt exist reconnect
                if(connection == null)
                {
                    var client = await channel.ConnectAsync();
                    connection = AddConnection(client, channel);
                }
            }
            else
            {
                var client = await channel.ConnectAsync();
                connection = AddConnection(client, channel);
            }

            return connection;
        }

        public async void HandleAudioConnection(IVoiceChannel channel)
        {
            var syncedClient = await SyncAudioConnectionsAsync(channel);
            //Launch something to download youtube video
            //Get path to download
            string path = @"C:\Users\corys\workspace\Discord.Net-Patch1.01\src\TestAudio\Start_the_game_already.mp3";

            Process p = CreateStream(path);

            await SendAsync(syncedClient, path);
            p.Kill();
        }

        private async Task SendAsync(AudioConnection connection, string path)
        {
            // Create FFmpeg using the previous example
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            {
                if(connection.stream == null)
                {
                    connection.stream = connection.client.CreatePCMStream(AudioApplication.Mixed);
                }
                try {
                    await connection.stream.FlushAsync();
                    await output.CopyToAsync(connection.stream); 
                }
                finally { await connection.stream.FlushAsync(); }
            }
            Console.WriteLine("Moo");
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}
