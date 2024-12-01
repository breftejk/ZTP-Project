using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

/// <summary>
/// DbContext for managing database connections and operations.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<WordPair> WordPairs { get; set; }
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

        base.OnModelCreating(modelBuilder);
    }
}