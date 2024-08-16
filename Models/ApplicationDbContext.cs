using Microsoft.EntityFrameworkCore;

namespace Quiz_API.Models
{
  public class ApplicationDbContext: DbContext
  {
    public DbSet<Card> Cards { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
  }
}
