using System;
using System.Collections.Generic;

#nullable disable

namespace DiscordBot.Models
{
    public partial class DiscordUser
    {
        public DiscordUser()
        {
            WordleRecords = new HashSet<WordleRecord>();
        }

        public Guid Id { get; set; }
        public decimal? DiscordId { get; set; }
        public string NickName { get; set; }

        public virtual ICollection<WordleRecord> WordleRecords { get; set; }
    }
}
