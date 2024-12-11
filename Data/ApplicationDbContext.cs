using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data
{
    /// <summary>
    /// Database context for the application, managing entities and their relationships.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupWord> GroupWords { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder used for defining entity configurations.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Word>()
                .HasOne(w => w.Language)
                .WithMany(l => l.Words)
                .HasForeignKey(w => w.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupWord>()
                .HasKey(gw => new { gw.GroupId, gw.WordId });

            modelBuilder.Entity<GroupWord>()
                .HasOne(gw => gw.Group)
                .WithMany(g => g.GroupWords)
                .HasForeignKey(gw => gw.GroupId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Language)
                .WithMany(l => l.Groups)
                .HasForeignKey(g => g.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupWord>()
                .HasOne(gw => gw.Word)
                .WithMany()
                .HasForeignKey(gw => gw.WordId);
        }
    }
}