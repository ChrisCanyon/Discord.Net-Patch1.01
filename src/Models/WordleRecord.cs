using System;
using System.Collections.Generic;

#nullable disable

namespace DiscordBot.Models
{
    public partial class WordleRecord
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public int? Score { get; set; }
        public DateTime? PostDate { get; set; }

        public virtual DiscordUser User { get; set; }
    }
}
