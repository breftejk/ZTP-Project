using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

/// <summary>
/// DbContext for managing database connections and operations.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<WordPair> WordPairs { get; set; }
    public DbSet<WordSet> WordSets { get; set; }
    public DbSet<Language> Languages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the primary key for Language
        modelBuilder.Entity<Language>()
            .HasKey(l => l.Code);

        // Configure the relationship between WordPair and Language via LanguageCode
        modelBuilder.Entity<WordPair>()
            .HasOne<Language>() // No navigation property to Language
            .WithMany()         // No collection of WordPairs in Language
            .HasForeignKey(wp => wp.LanguageCode)
            .HasPrincipalKey(l => l.Code) // Relationship mapped to Language.Code
            .OnDelete(DeleteBehavior.Cascade); // Deleting a language deletes its WordPairs

// Configure the many-to-many relationship between WordSet and WordPair
        modelBuilder.Entity<WordSet>()
            .HasMany(ws => ws.WordPairs)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "WordSetWordPair",
                j => j
                    .HasOne<WordPair>()
                    .WithMany()
                    .HasForeignKey("WordPairId")
                    .HasConstraintName("FK_WordSetWordPair_WordPairs_WordPairId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<WordSet>()
                    .WithMany()
                    .HasForeignKey("WordSetId")
                    .HasConstraintName("FK_WordSetWordPair_WordSets_WordSetId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("WordSetId", "WordPairId");
                    j.ToTable("WordSetWordPair");
                });
        
        // Configure the one-to-many relationship between User and WordSet
        modelBuilder.Entity<User>()
            .HasMany(u => u.WordSets)
            .WithOne(ws => ws.User)
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}