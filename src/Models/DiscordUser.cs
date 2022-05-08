using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DiscordBot.Models
{
    [Table("DiscordUser")]
    public partial class DiscordUser
    {
        public DiscordUser()
        {
            WordleRecords = new HashSet<WordleRecord>();
        }

        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "numeric(19, 0)")]
        public decimal? DiscordId { get; set; }
        [StringLength(50)]
        public string NickName { get; set; }

        [InverseProperty(nameof(WordleRecord.User))]
        public virtual ICollection<WordleRecord> WordleRecords { get; set; }
    }
}
