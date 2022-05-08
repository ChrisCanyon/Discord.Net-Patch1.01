using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DiscordBot.Models
{
    [Table("WordleRecord")]
    public partial class WordleRecord
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public int? Score { get; set; }
        public int? WordleNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PostDate { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(DiscordUser.WordleRecords))]
        public virtual DiscordUser User { get; set; }
    }
}
