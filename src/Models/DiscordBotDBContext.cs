using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DiscordBot.Models
{
    public partial class DiscordBotDBContext : DbContext
    {
        public DiscordBotDBContext()
        {
        }

        public DiscordBotDBContext(DbContextOptions<DiscordBotDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DiscordUser> DiscordUsers { get; set; }
        public virtual DbSet<WordleRecord> WordleRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DiscordUser>(entity =>
            {
                entity.ToTable("DiscordUser");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DiscordId).HasColumnType("numeric(19, 0)");

                entity.Property(e => e.NickName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WordleRecord>(entity =>
            {
                entity.ToTable("WordleRecord");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.PostDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WordleRecords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__WordleRec__UserI__286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
