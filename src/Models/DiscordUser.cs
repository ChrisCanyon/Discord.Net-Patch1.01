using System;
using System.Collections.Generic;

/*
 * THIS IS AN AUTO GENERATED FILE CHANGES WILL BE OVERWRITTEN
 * 
 */

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
        public UInt64 DiscordId { get; set; }
        public string NickName { get; set; }
        public virtual ICollection<WordleRecord> WordleRecords { get; set; }
    }
}
