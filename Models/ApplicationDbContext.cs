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
      modelBuilder.Entity<DeckCard>()
          .HasKey(dc => new { dc.DeckId, dc.CardId });

      modelBuilder.Entity<DeckCard>()
        .HasOne(dc => dc.QuizDeck)
        .WithMany(qd => qd.DeckCards)
        .HasForeignKey(dc => dc.DeckId);

      modelBuilder.Entity<DeckCard>()
          .HasOne(dc => dc.Card)
          .WithMany(c => c.DeckCards)
          .HasForeignKey(dc => dc.CardId);

      base.OnModelCreating(modelBuilder);
    }

  }
}
