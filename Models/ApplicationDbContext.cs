using Microsoft.EntityFrameworkCore;

namespace Quiz_API.Models
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<UserAuth> UserAuths { get; set; }
    public DbSet<QuizDeck> QuizDecks { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<DeckCard> DeckCards { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Add any specific configurations here if necessary
      modelBuilder.Entity<DeckCard>()
          .HasKey(dc => new { dc.DeckId, dc.CardId });

      modelBuilder.Entity<DeckCard>()
          .HasIndex(dc => new { dc.DeckId, dc.OrderIndex })
          .IsUnique();

      base.OnModelCreating(modelBuilder);
    }

  }
}
