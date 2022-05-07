using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Helpers
{
    class UserLoggingHelper
    {
        public static void LogUserMessage(SocketCommandContext socketCommand, DbContext dbContext)
        {
            var user = socketCommand.User;

            UpdateUser(user, dbContext);
        }

        private static void UpdateUser(SocketUser user, DbContext dbContext)
        {

        }
    }
}
