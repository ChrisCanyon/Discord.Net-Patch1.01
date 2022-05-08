using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DiscordBot.Modules
{
    [Name("Wordle")]
    public class WordleModule : ModuleBase<SocketCommandContext>
        {
            private DiscordBotDBContext DBContext;
            private DiscordSocketClient DiscordClient;
            private VoiceConnectivityService VoiceService;

            public WordleModule(DiscordSocketClient discord, DiscordBotDBContext context, VoiceConnectivityService voice)
            {
                DBContext = context;
                DiscordClient = discord;
                VoiceService = voice;
            }

        [Command("Wordle")]
        [Summary("Record Wordle Score")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task Wordle([Remainder] string text)
        {
            int? wordleNumber = GetWordleNumber(text);
            int? wordleScore = GetWordleScoreFromText(text);

            if (wordleScore.HasValue)
            {

            }


        }

        public int? GetWordleScoreFromText(string input)
        {
            int score;

            try
            {
                string result = input.Split(" ")[1].Split("/")[0];
                if (result == "X")
                {
                    result = "7";
                }
                score = Convert.ToInt32(result);
            }

            catch (Exception ex)
            {
                return null;
            }

            return score;
        }

        public int? GetWordleNumber(string input)
        {
            try
            {
                return Convert.ToInt32(input.Split(" ")[0]);
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public void AddScoreToDB(int score, int wordleNum, SocketGuildUser thisUser)
        {
            var user = DBContext.DiscordUsers.AsQueryable().Where(x => x.DiscordId == thisUser.Id).FirstOrDefault();
            if (user == null)
            {
                user = new DiscordUser();
                user.DiscordId = thisUser.Id;
                DBContext.Add(user);
            }

            WordleRecord newRecord = new WordleRecord();
            newRecord.UserId = user.Id;
            newRecord.Score = score;
            newRecord.WordleNumber = wordleNum;
            newRecord.PostDate = DateTime.Now;

            DBContext.Add(newRecord);
            DBContext.SaveChanges();
        }

        /*public bool CheckDuplicate(int number, SocketGuildUser thisUser)
        {

        }*/

    }
}
