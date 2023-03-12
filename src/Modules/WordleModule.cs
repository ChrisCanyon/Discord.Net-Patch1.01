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

            if (wordleScore.HasValue && wordleNumber.HasValue)
            {
                AddScoreToDB(wordleScore.Value, wordleNumber.Value);
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

        public void AddScoreToDB(int score, int wordleNum)
        {
            //TODO Consider breaking this out into a function or service
            //try to find the user who posted
            var user = DBContext.DiscordUsers.AsQueryable().Where(x => x.DiscordId == Context.User.Id).FirstOrDefault();
            if (user == null)
            {
                //create a new record if they don't exist
                user = new DiscordUser();
                user.DiscordId = Context.User.Id;
                user.NickName = Context.User.Username;
                DBContext.DiscordUsers.Add(user);
            }

            //create a new wordle record
            WordleRecord newRecord = new WordleRecord();
            newRecord.User = user;
            newRecord.Score = score;
            newRecord.WordleNumber = wordleNum;
            newRecord.PostDate = DateTime.Now;

            DBContext.WordleRecords.Add(newRecord);
            DBContext.SaveChanges();
        }

        /*public bool CheckDuplicate(int number, SocketGuildUser thisUser)
        {

        }*/

    }
}
