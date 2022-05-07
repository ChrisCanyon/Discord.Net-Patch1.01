using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Discord;
using Newtonsoft.Json;
using Discord.Net;

namespace DiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfiguration, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IConfiguration config,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
            _discord.SlashCommandExecuted += SlashCommandHandler;
            _discord.Ready += Client_Ready;

        }

        public async Task Client_Ready()
        {
            SetupSlashCommands();
        }

        private async void SetupSlashCommands()
        {
            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            var guilds = _discord.Guilds.GetEnumerator();
            guilds.MoveNext();
            var guild = guilds.Current;


            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var guildCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            guildCommand.WithName("first-command");

            // Descriptions can have a max length of 100.
            guildCommand.WithDescription("This is my first guild slash command!");

            // Let's do our global command
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("first-global-command");
            globalCommand.WithDescription("This is my first global slash command");

            

            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
             //   await guild.CreateApplicationCommandAsync(guildCommand.Build());

                // With global commands we don't need the guild.
                await _discord.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }

        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands
            
            var context = new SocketCommandContext(_discord, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);     // Execute the command

                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
