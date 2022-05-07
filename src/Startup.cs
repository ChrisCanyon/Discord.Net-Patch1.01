using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Extensions.Configuration.SystemsManager;


namespace DiscordBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
                .AddYamlFile("_config.yml");                // Add this (yaml encoded) file to the configuration
            Configuration = builder.Build();                // Build the configuration
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();             // Create a new instance of a service collection
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();     // Build the service provider
            provider.GetRequiredService<LoggingService>();      // Start the logging service
            provider.GetRequiredService<CommandHandler>(); 		// Start the command handler service

            await provider.GetRequiredService<StartupService>().StartAsync();       // Start the startup service
            await Task.Delay(-1);                               // Keep the program alive
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000             // Cache 1,000 messages per channel
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {                                       // Add the command service to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
            }))
#if DEBUG
            .AddDbContext<DiscordBotDBContext>(options =>
                options.UseSqlServer(Configuration["connection_string:local"])) //local DB
#else
            .AddDbContext<DiscordBotDBContext>(options =>
                options.UseSqlServer(AWSHelper.GetRDSConnectionString(Configuration))) //AWS RDS
#endif
            .AddSingleton<CommandHandler>()         // Add the command handler to the collection
            .AddSingleton<VoiceConnectivityService>()
            .AddSingleton<StartupService>()         // Add startupservice to the collection
            .AddSingleton<LoggingService>()         // Add loggingservice to the collection
            .AddSingleton<Random>()                 // Add random to the collection
            .AddSingleton(Configuration);           // Add the configuration to the collection
        }
    }
}
