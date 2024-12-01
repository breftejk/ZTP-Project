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

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}